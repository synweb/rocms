CREATE PROCEDURE [News].[RssProcessedItem_Update]
	@RssProcessedItemId int,
	@RssSource nvarchar(4000),
	@NewsItemId INT
AS
	UPDATE [News].[RssProcessedItem] 
		SET
		 [RssSource]=@RssSource,
		 [NewsItemId]=@NewsItemId
		WHERE
			[RssProcessedItemId] = @RssProcessedItemId
