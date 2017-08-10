CREATE PROCEDURE [dbo].[Album_Select]
AS
	SELECT [AlbumId], [Name], [Description], [CreationDate], [OwnerId] FROM [dbo].[Album]
