CREATE PROCEDURE [dbo].[Video_Select]
AS
	SELECT [VideoId], [AlbumId], [ImageId], [CreationDate], [Title], [Description], [SortOrder]
	FROM [dbo].[Video]
