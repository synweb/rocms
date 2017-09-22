CREATE PROCEDURE [News].[NewsItem_SelectIds]
	@OnlyPosted bit
AS
	SELECT [HeartId] FROM [News].[NewsItem]
		WHERE @OnlyPosted=0 OR [PostingDate]<=GETUTCDATE()
		ORDER BY [PostingDate] DESC