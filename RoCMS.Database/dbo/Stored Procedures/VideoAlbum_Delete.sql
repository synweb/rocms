CREATE PROCEDURE [dbo].[VideoAlbum_Delete]
@AlbumId INT
AS
	DELETE FROM [dbo].[VideoAlbum]
	WHERE [AlbumId]=@AlbumId
