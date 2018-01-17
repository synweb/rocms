CREATE PROCEDURE [dbo].[Album_Update]
@Name nvarchar(50),
@Description nvarchar(MAX),
@OwnerId int,
@WatermarkImageId varchar(30),
@AlbumId int
AS
	UPDATE [dbo].[Album] SET
		[Name]=@Name,
		[Description]=@Description,
		[OwnerId]=@OwnerId,
		[WatermarkImageId]=@WatermarkImageId
	WHERE [AlbumId]=@AlbumId
