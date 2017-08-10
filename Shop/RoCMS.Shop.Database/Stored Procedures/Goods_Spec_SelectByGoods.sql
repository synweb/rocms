CREATE PROCEDURE [Shop].[Goods_Spec_SelectByGoods]
@GoodsId int
AS
	SELECT * FROM [Shop].[Goods_Spec]
		WHERE [GoodsId]=@GoodsId