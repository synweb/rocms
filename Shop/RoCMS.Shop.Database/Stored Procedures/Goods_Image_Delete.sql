CREATE PROCEDURE [Shop].[Goods_Image_Delete]
@HeartId int,
@ImageId varchar(30)
AS
	DELETE FROM [Shop].[Goods_Image]
	WHERE [HeartId]=@HeartId
		 AND [ImageId]=@ImageId
