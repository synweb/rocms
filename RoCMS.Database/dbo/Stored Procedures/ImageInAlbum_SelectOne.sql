CREATE PROCEDURE [dbo].[ImageInAlbum_SelectOne]
	@AlbumId int,
	@ImageId varchar(30)
AS
	SELECT * FROM [ImageInAlbum] WHERE [AlbumId]=@AlbumId AND [ImageId]=@ImageId
