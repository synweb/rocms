CREATE PROCEDURE [dbo].[Album_Delete]
@AlbumId int
AS
	DELETE FROM [dbo].[Album]
	WHERE [AlbumId]=@AlbumId
