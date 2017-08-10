CREATE PROCEDURE [Shop].[Goods_Image_Delete]
@GoodsId int,
@ImageId varchar(30)
AS
	DELETE FROM [Shop].[Goods_Image]
	WHERE [GoodsId]=@GoodsId
		 AND [ImageId]=@ImageId
