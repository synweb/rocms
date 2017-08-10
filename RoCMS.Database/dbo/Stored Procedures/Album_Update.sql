CREATE PROCEDURE [dbo].[Album_Update]
@Name nvarchar(50),
@Description nvarchar(MAX),
@OwnerId int,
@AlbumId int
AS
	UPDATE [dbo].[Album] SET
		[Name]=@Name,
		[Description]=@Description,
		[OwnerId]=@OwnerId
	WHERE [AlbumId]=@AlbumId
