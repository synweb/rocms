CREATE PROCEDURE [Shop].[CompatibleSet_Goods_SelectByGoods]
@GoodsId int
AS
	SELECT * FROM [Shop].[CompatibleSet_Goods]
		WHERE GoodsId=@GoodsId
