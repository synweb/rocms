CREATE PROCEDURE [dbo].[ImageInAlbum_SelectCountByAlbum]
	@AlbumId int
AS
	SELECT COUNT(*) FROM [ImageInAlbum] WHERE [AlbumId]=@AlbumId
