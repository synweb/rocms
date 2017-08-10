CREATE PROCEDURE [Shop].[Goods_Image_SelectByGoods]
@GoodsId int
AS
	SELECT * FROM [Shop].[Goods_Image]
		WHERE GoodsId=@GoodsId
