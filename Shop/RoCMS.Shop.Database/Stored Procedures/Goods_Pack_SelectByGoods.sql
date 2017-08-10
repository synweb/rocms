CREATE PROCEDURE [Shop].[Goods_Pack_SelectByGoods]
@GoodsId int
AS
	SELECT * FROM [Shop].[Goods_Pack]
		WHERE [GoodsId]=@GoodsId