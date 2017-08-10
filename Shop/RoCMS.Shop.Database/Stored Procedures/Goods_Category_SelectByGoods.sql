CREATE PROCEDURE [Shop].[Goods_Category_SelectByGoods]
@GoodsId int
AS
	SELECT * FROM [Shop].[Goods_Category]
		WHERE [GoodsId]=@GoodsId
