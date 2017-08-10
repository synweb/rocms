CREATE PROCEDURE [dbo].[VideoAlbum_Update]
@AlbumId INT,
@Name  NVARCHAR (50),
@CreationDate DATETIME,
@OwnerId INT
AS
	UPDATE [dbo].[VideoAlbum]
	SET [Name]=@Name, 
		[CreationDate]=@CreationDate, 
		[OwnerId]=@OwnerId
	WHERE [AlbumId] = @AlbumId
