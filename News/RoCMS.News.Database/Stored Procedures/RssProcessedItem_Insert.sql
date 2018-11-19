CREATE PROCEDURE [News].[RssProcessedItem_Insert]
	@RssSource nvarchar(4000),
	@NewsItemId INT
AS
	INSERT INTO [News].[RssProcessedItem] (RssSource, NewsItemId) 
		VALUES (@RssSource, @NewsItemId)
	SELECT @@IDENTITY
