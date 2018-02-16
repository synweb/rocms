CREATE PROCEDURE [Shop].[GoodsItem_Delete]
@HeartId int
AS
	UPDATE [Shop].[GoodsItem]
	SET Deleted=1
	WHERE [HeartId]=@HeartId
