﻿CREATE PROCEDURE [Shop].[Goods_Filter]
    @CategoryIds [Int_Table] readonly,
    @WithSubcategories bit,
    @ManufacturerIds [Int_Table] readonly,
    @SupplierIds [Int_Table] readonly,
    @Countries [Int_Table] readonly,
    @ActionIds [Int_Table] readonly,
    @PackIds [Int_Table] readonly,
    @SpecIds [Int_String_Table] readonly,
    @SearchQuery NVARCHAR(500),
    @FulltextSearchQuery NVARCHAR(2000),

	@MinPrice decimal,
	@MaxPrice decimal,

    @SortBy VARCHAR(20),
    @SortOrder VARCHAR(4),
    
    @StartIndex INT, -- deprecated
    @Count INT, -- deprecated
    @TotalCount INT OUTPUT
AS
    
    
DECLARE @CategoryIdsExist BIT = 0, @ManufacturerIdsExist BIT = 0, 
    @CountriesExist BIT = 0, @ActionIdsExist BIT = 0, @PackIdsExist BIT = 0, @SpecIdsExist BIT = 0, @SupplierIdsExist BIT = 0

IF EXISTS (SELECT * FROM @CategoryIds) 
    SET @CategoryIdsExist=1
IF EXISTS (SELECT * FROM @ManufacturerIds) 
    SET @ManufacturerIdsExist=1
IF EXISTS (SELECT * FROM @SupplierIds) 
    SET @SupplierIdsExist=1
IF EXISTS (SELECT * FROM @Countries) 
    SET @CountriesExist=1
IF EXISTS (SELECT * FROM @ActionIds) 
    SET @ActionIdsExist=1
IF EXISTS (SELECT * FROM @PackIds) 
    SET @PackIdsExist=1
IF EXISTS (SELECT * FROM @SpecIds) 
    SET @SpecIdsExist=1
SET NOCOUNT ON
--SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @HeartIds TABLE(RowNumber INT IDENTITY PRIMARY KEY, HeartId INT)

DECLARE @FullCategoryIds TABLE(Val INT)

IF @WithSubcategories = 1
BEGIN
    ;WITH ret AS(
            SELECT	[HeartId], [ParentCategoryId]
            FROM	[Shop].[Category]
            WHERE	([HeartId] IN (SELECT Val FROM @CategoryIds)) -- OR (NOT EXISTS (SELECT * FROM @CategoryIds))
            UNION ALL
            SELECT	t.[HeartId], t.[ParentCategoryId]
            FROM	[Shop].[Category] t INNER JOIN
                    ret r ON t.[ParentCategoryId] = r.[HeartId]
    )
    INSERT INTO @FullCategoryIds
    SELECT HeartId
    FROM ret
END
ELSE BEGIN
    INSERT INTO @FullCategoryIds
    SELECT [Val]
    FROM @CategoryIds
END

-- Вытаскиваем айдишники товаров с акциями
DECLARE @ActionHeartIds [Int_Table]
IF @ActionIdsExist=1
BEGIN
    DECLARE @ActionCategoryIds [Int_Table] 
    INSERT INTO @ActionCategoryIds (Val)
    SELECT CategoryId FROM [Shop].[Action_Category] WHERE [ActionId] IN (SELECT Val FROM @ActionIds) 

    DECLARE @ActionFullCategoryIds [Int_Table] 
    DECLARE @ActionManufacturerIds [Int_Table]

    ;WITH ret AS(
            SELECT	*
            FROM	[Shop].[Category]
            WHERE	([HeartId] IN (SELECT Val FROM @ActionCategoryIds))
            UNION ALL
            SELECT	t.*
            FROM	[Shop].[Category] t INNER JOIN
                    ret r ON t.[ParentCategoryId] = r.[HeartId]
    )
    INSERT INTO @ActionFullCategoryIds (Val)
    SELECT HeartId
    FROM ret

    
    INSERT INTO @ActionManufacturerIds (Val)
    SELECT [ManufacturerId] FROM [Shop].[Action_Manufacturer] WHERE [ActionId] IN (SELECT Val FROM @ActionIds)
    
    
    INSERT INTO @ActionHeartIds (Val)
    SELECT HeartId FROM [Shop].[Action_Goods] WHERE ActionId IN (SELECT Val FROM @ActionIds)
    UNION
    SELECT HeartId FROM [Shop].[GoodsItem] WHERE [ManufacturerId] IN (SELECT Val FROM @ActionManufacturerIds)
    UNION
    SELECT GoodsId FROM [Shop].Goods_Category WHERE CategoryId IN (SELECT Val FROM @ActionFullCategoryIds)
END


DECLARE @FinalManufacturerIds [Int_Table]

DECLARE @CountryManufacturerIds [Int_Table]
IF @CountriesExist=1
BEGIN
    INSERT INTO @CountryManufacturerIds (Val)
    SELECT [HeartId] FROM [Shop].[Manufacturer] WHERE [CountryId] IN (SELECT Val FROM @Countries)
END

IF @CountriesExist=1 OR @ManufacturerIdsExist = 1
BEGIN
INSERT INTO @FinalManufacturerIds (Val)
    SELECT Val FROM (
        SELECT *, COUNT(*) cnt FROM (
        SELECT * FROM @CountryManufacturerIds
        UNION ALL
        SELECT * FROM @ManufacturerIds
        ) t GROUP BY Val
        ) t2
        GROUP BY  cnt, Val
        HAVING cnt = (SELECT MAX(cnt) FROM (SELECT *, COUNT(*) cnt FROM (
    SELECT * FROM @CountryManufacturerIds
        UNION ALL
        SELECT * FROM @ManufacturerIds
        ) t GROUP BY Val
        ) t3)
END

DECLARE @UnsortedHeartIds [Int_Table]

INSERT INTO @UnsortedHeartIds
SELECT DISTINCT gc1.GoodsId
FROM [Shop].[Goods_Category] gc1 join [GoodsItem] g on gc1.GoodsId = g.HeartId
WHERE
g.Deleted=0
AND
(@CategoryIdsExist = 0 OR EXISTS (SELECT * FROM @FullCategoryIds WHERE Val=gc1.CategoryId)) --gc1.CategoryId IN (SELECT Val FROM @FullCategoryIds))
AND
((
	@MaxPrice IS NULL AND @MinPrice IS NOT NULL AND EXISTS(SELECT * FROM GoodsWithActualDiscounts gwad WHERE  gwad.HeartId = gc1.GoodsId AND (gwad.ActualPrice >= @MinPrice OR gwad.ActualPrice = 0))
)
OR
(
	@MaxPrice IS NULL OR EXISTS(SELECT * FROM GoodsWithActualDiscounts gwad WHERE  gwad.HeartId = gc1.GoodsId AND gwad.ActualPrice > 0 AND gwad.ActualPrice <= @MaxPrice)
)
AND
(
	@MinPrice IS NULL OR EXISTS(SELECT * FROM GoodsWithActualDiscounts gwad WHERE  gwad.HeartId = gc1.GoodsId AND gwad.ActualPrice >= @MinPrice)
))            
AND
( (@ManufacturerIdsExist = 0 AND @CountriesExist = 0) OR
        
    EXISTS (SELECT * FROM [GoodsItem] g WHERE g.HeartId = gc1.GoodsId AND (
    g.SupplierId IN (SELECT Val FROM @FinalManufacturerIds) 
    OR g.ManufacturerId IN (SELECT Val FROM @FinalManufacturerIds))))
AND
(@SupplierIdsExist = 0 OR g.SupplierId IN (SELECT Val FROM @SupplierIds))
AND
(@ActionIdsExist = 0 OR gc1.GoodsId IN (SELECT Val FROM @ActionHeartIds))
AND
(@PackIdsExist = 0 OR gc1.GoodsId IN (SELECT HeartId FROM [Goods_Pack] WHERE [PackId] IN (SELECT Val FROM @PackIds) ))
    AND
(@SpecIdsExist = 0
OR (SELECT COUNT(*) FROM @SpecIds) = (SELECT Count(*) From [Goods_Spec] gs INNER JOIN @SpecIds sis ON gs.SpecId = sis.[Key] AND [Shop].CheckSpecValue(sis.[Val], gs.[Value]) = 1 AND gs.HeartId = gc1.GoodsId))
--OR gc1.GoodsId IN (SELECT HeartId From [Goods_Spec] gs INNER JOIN @SpecIds sis ON gs.SpecId = sis.[Key] AND [Shop].CheckSpecValue(sis.[Val], gs.[Value]) = 1))
--OPTION (OPTIMIZE FOR (@CategoryIdsExist=1))

IF (@SearchQuery IS NOT NULL AND @SearchQuery != '' AND @FulltextSearchQuery IS NOT NULL AND @FulltextSearchQuery != '')
BEGIN
    DECLARE @searchHeartIds [Int_Table]
    INSERT INTO @searchHeartIds
    SELECT HeartId FROM (
        SELECT g.HeartId, 1 AS ColumnLocation, 2000 AS Rank --Entire Fit by article 
        FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId WHERE LOWER([Article])=@SearchQuery
        UNION
        SELECT g.HeartId, 2 AS ColumnLocation, 1800 AS Rank --Entire Fit by name
        FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId WHERE LOWER([Name])=@SearchQuery
        UNION
        SELECT g.HeartId, 3 AS ColumnLocation, Rank --Partial Fit by article
        FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId INNER JOIN
           CONTAINSTABLE([Shop].[GoodsItem], 
            [Name], 
            @FulltextSearchQuery
           ) AS KEY_TBL
           ON g.HeartId = KEY_TBL.[KEY]
        UNION
        SELECT g.HeartId, 4 AS ColumnLocation, Rank --Partial Fit by description
        FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId INNER JOIN
           CONTAINSTABLE([Shop].[GoodsItem], 
            [HtmlDescription], 
            @FulltextSearchQuery
           ) AS KEY_TBL
           ON g.HeartId = KEY_TBL.[KEY]
        ) t
    ORDER BY RANK DESC

    DELETE FROM @UnsortedHeartIds
    INSERT INTO @UnsortedHeartIds -- нужно для сохранения порядка по релевантности
    SELECT DISTINCT Val FROM @searchHeartIds
END
        
SELECT @TotalCount = COUNT(*) FROM @UnsortedHeartIds
--SELECT @TotalCount

-- Сортируем

IF @SortBy='Relevance'
BEGIN
    INSERT INTO @HeartIds (HeartId)
    SELECT Val FROM @UnsortedHeartIds ug-- JOIN [GoodsItem] g ON ug.Val = g.HeartId
END
ELSE IF @SortBy='CreationDate'
BEGIN
    IF @SortOrder = 'Asc'
    BEGIN
        INSERT INTO @HeartIds (HeartId)
        SELECT Val FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId
            ORDER BY g.NotAvailable, ug.Val
    END
    ELSE
    BEGIN
        INSERT INTO @HeartIds (HeartId)
        SELECT Val FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId
            ORDER BY g.NotAvailable, ug.Val DESC
    END
END
ELSE IF @SortBy='Article'
BEGIN
    INSERT INTO @HeartIds (HeartId)
    SELECT Val FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId
        ORDER BY g.NotAvailable, g.[Article]
END
ELSE IF @SortBy='Random'
BEGIN
    INSERT INTO @HeartIds (HeartId)
    SELECT Val FROM @UnsortedHeartIds ug JOIN [GoodsItem] g ON ug.Val = g.HeartId
        ORDER BY g.NotAvailable, NEWID()
END
ELSE IF @SortBy='Price'
BEGIN

    IF @SortOrder = 'Asc'
    BEGIN
        INSERT INTO @HeartIds (HeartId)
        SELECT Val FROM @UnsortedHeartIds ug JOIN GoodsWithActualDiscounts g ON ug.Val = g.HeartId
            ORDER BY g.NotAvailable, g.[ActualPrice]
    END
    ELSE
    BEGIN
        INSERT INTO @HeartIds (HeartId)
        SELECT Val FROM @UnsortedHeartIds ug JOIN GoodsWithActualDiscounts g ON ug.Val = g.HeartId
            ORDER BY g.NotAvailable, g.[ActualPrice] DESC
    END
END
ELSE IF @SortBy='Rating'
BEGIN
    IF @SortOrder = 'Asc'
    BEGIN
        INSERT INTO @HeartIds (HeartId)
        SELECT Val FROM @UnsortedHeartIds ug LEFT JOIN GoodsReview gr ON ug.Val = gr.HeartId INNER JOIN GoodsItem g ON ug.Val = g.HeartId
            GROUP BY Val, g.NotAvailable
            ORDER BY g.NotAvailable, AVG(Rating)
    END
    ELSE
    BEGIN
        INSERT INTO @HeartIds (HeartId)
        SELECT Val FROM @UnsortedHeartIds ug LEFT JOIN GoodsReview gr ON ug.Val = gr.HeartId INNER JOIN GoodsItem g ON ug.Val = g.HeartId
            GROUP BY Val, g.NotAvailable
            ORDER BY g.NotAvailable, AVG(Rating) DESC
    END
END



SELECT HeartId FROM @HeartIds
ORDER BY RowNumber
--CASE WHEN @SortOrder = 'Asc' THEN RowNumber else -RowNumber END asc
-- OFFSET (@StartIndex-1) ROWS
-- FETCH NEXT @Count ROWS ONLY --g JOIN [GoodsWithActualDiscounts] gd ON g.HeartId = gd.HeartId

--SET TRANSACTION ISOLATION LEVEL READ COMMITTED