CREATE PROCEDURE [Shop].[Goods_Image_Insert]
@GoodsId int,
@ImageId varchar(30)
AS
	INSERT INTO [Shop].[Goods_Image] ([GoodsId], [ImageId])
	VALUES (@GoodsId, @ImageId)
