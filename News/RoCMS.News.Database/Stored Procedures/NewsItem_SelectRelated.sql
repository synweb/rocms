CREATE PROCEDURE [News].[NewsItem_SelectRelated]
	@NewsId int,
	@WithSubnews bit,
	@Count int,
	@OnlyPosted bit
AS

DECLARE @FullNewsIds TABLE(Val INT)

IF @WithSubnews = 1
BEGIN
	;WITH ret AS(
    		SELECT	[NewsId], [RelatedNewsItemId]
    		FROM	[NewsItem]
    		WHERE	[RelatedNewsItemId] = @NewsId
    		UNION ALL
    		SELECT	t.[NewsId], t.[RelatedNewsItemId]
    		FROM	[NewsItem] t INNER JOIN
    				ret r ON t.[RelatedNewsItemId] = r.[NewsId]
	)
	INSERT INTO @FullNewsIds
	SELECT NewsId
	FROM ret
END
ELSE BEGIN
	INSERT INTO @FullNewsIds
	SELECT [NewsId] FROM [NewsItem]
	WHERE [RelatedNewsItemId] = @NewsId
END


SELECT TOP(@Count) NewsId FROM [NewsItem] 
WHERE NewsId IN (SELECT * FROM @FullNewsIds) AND (@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE()) 
ORDER BY PostingDate DESC 