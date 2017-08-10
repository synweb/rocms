CREATE PROCEDURE [dbo].[VideoAlbum_SelectOne]
@AlbumId INT
AS
	SELECT [AlbumId], [Name], [CreationDate], [OwnerId]
	FROM [dbo].[VideoAlbum]
	WHERE [AlbumId] = @AlbumId
