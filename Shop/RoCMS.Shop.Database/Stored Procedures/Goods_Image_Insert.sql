CREATE PROCEDURE [Shop].[Goods_Image_Insert]
@HeartId int,
@ImageId varchar(30)
AS
	INSERT INTO [Shop].[Goods_Image] ([HeartId], [ImageId])
	VALUES (@HeartId, @ImageId)
