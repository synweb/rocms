CREATE PROCEDURE [News].[RssProcessedItem_Delete]
	@RssProcessedItemId int
AS
	DELETE FROM [News].[RssProcessedItem] WHERE [RssProcessedItemId]=@RssProcessedItemId
