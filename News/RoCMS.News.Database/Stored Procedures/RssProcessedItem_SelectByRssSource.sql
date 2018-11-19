CREATE PROCEDURE [News].[RssProcessedItem_SelectByRssSource]
	@RssSource nvarchar(max)
AS
	SELECT * from [News].[RssProcessedItem] where [RssSource]=@RssSource
