CREATE PROCEDURE [dbo].[Album_SelectUserAlbums]
	@OwnerId int
AS
	SELECT * FROM [Album] 
		WHERE (@OwnerId IS NULL AND [OwnerId] IS NOT NULL) OR (OwnerId = @OwnerId)