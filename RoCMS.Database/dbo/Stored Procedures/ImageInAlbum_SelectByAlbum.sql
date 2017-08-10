CREATE PROCEDURE [dbo].[ImageInAlbum_SelectByAlbum]
	@AlbumId int
AS
	SELECT * FROM [ImageInAlbum] WHERE [AlbumId]=@AlbumId
