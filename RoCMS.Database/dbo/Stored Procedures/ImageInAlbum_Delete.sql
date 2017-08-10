CREATE PROCEDURE [dbo].[ImageInAlbum_Delete]
@AlbumId int,
@ImageId varchar(30)
AS
	DELETE FROM [dbo].[ImageInAlbum]
	WHERE [AlbumId]=@AlbumId
		 AND [ImageId]=@ImageId
