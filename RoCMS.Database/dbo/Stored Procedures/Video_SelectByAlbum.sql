CREATE PROCEDURE [dbo].[Video_SelectByAlbum]
@AlbumId int
AS
	SELECT [VideoId], [AlbumId], [ImageId], [CreationDate], [Title], [Description], [SortOrder]
	FROM [dbo].[Video]
	WHERE [AlbumId]=@AlbumId
