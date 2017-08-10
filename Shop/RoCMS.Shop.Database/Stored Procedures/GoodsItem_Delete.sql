CREATE PROCEDURE [Shop].[GoodsItem_Delete]
@GoodsId int
AS
	UPDATE [Shop].[GoodsItem]
	SET Deleted=1
	WHERE [GoodsId]=@GoodsId
