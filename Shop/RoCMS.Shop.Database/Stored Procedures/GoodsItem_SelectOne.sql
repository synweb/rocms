CREATE PROCEDURE [Shop].[GoodsItem_SelectOne]
@GoodsId int
AS
	SELECT * FROM [Shop].[GoodsItem]
	WHERE [GoodsId]=@GoodsId AND Deleted=0
