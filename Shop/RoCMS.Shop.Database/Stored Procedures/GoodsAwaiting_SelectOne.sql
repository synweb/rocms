CREATE PROCEDURE [Shop].[GoodsAwaiting_SelectOne]
@GoodsAwaitingId int
AS
	SELECT * FROM [Shop].[GoodsAwaiting]
	WHERE [GoodsAwaitingId]=@GoodsAwaitingId