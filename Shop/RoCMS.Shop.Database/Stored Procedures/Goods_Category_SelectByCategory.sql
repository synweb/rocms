CREATE PROCEDURE [Shop].[Goods_Category_SelectByCategory]
@CategoryId int
AS
	SELECT * FROM [Shop].[Goods_Category]
		WHERE [CategoryId]=@CategoryId
