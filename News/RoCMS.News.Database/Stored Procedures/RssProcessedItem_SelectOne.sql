CREATE PROCEDURE [News].[RssProcessedItem_SelectOne]
	@RssProcessedItemId int
AS
	SELECT * FROM [News].[RssProcessedItem] WHERE [RssProcessedItemId]=@RssProcessedItemId
