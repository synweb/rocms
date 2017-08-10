CREATE PROCEDURE [Shop].[GoodsInOrder_SelectByGoods]
@GoodsId int
AS
	SELECT * FROM [Shop].[GoodsInOrder]
		WHERE [GoodsId]=@GoodsId