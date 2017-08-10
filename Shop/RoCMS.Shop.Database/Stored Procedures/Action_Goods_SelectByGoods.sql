CREATE PROCEDURE [Shop].[Action_Goods_SelectByGoods]
@GoodsId int
AS
	SELECT * FROM [Shop].[Action_Goods]
		WHERE [GoodsId]=@GoodsId
