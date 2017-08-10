CREATE PROCEDURE [Shop].[GoodsAwaiting_Delete]
@GoodsAwaitingId int
AS
	DELETE FROM [Shop].[GoodsAwaiting]
	WHERE [GoodsAwaitingId]=@GoodsAwaitingId
