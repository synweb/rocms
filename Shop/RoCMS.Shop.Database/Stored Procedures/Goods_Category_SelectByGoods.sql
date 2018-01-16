CREATE PROCEDURE [Shop].[Goods_Category_SelectByGoods]
@HeartId int
AS
	SELECT * FROM [Shop].[Goods_Category]
		WHERE [GoodsHeartId]=@HeartId
