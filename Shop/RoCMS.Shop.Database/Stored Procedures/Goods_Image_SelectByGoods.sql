CREATE PROCEDURE [Shop].[Goods_Image_SelectByGoods]
@HeartId int
AS
	SELECT * FROM [Shop].[Goods_Image]
		WHERE HeartId=@HeartId
