CREATE PROCEDURE [Shop].[Goods_Image_Update]
@HeartId int,
@ImageId varchar(30),
@SortOrder int
AS

UPDATE [Shop].Goods_Image
SET [SortOrder] = @SortOrder
WHERE HeartId = @HeartId AND ImageId = @ImageId
