CREATE PROCEDURE [dbo].[VideoAlbum_Select]
AS
	SELECT [AlbumId], [Name], [CreationDate], [OwnerId]
	FROM [dbo].[VideoAlbum]
