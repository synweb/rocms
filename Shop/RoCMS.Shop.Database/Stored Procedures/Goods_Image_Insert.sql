CREATE PROCEDURE [Shop].[Goods_Image_Insert]
@HeartId int,
@ImageId varchar(30),
@SortOrder int
AS
	INSERT INTO [Shop].[Goods_Image] ([HeartId], [ImageId], [SortOrder])
	VALUES (@HeartId, @ImageId, @SortOrder)
