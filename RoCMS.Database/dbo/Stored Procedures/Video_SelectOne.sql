CREATE PROCEDURE [dbo].[Video_SelectOne]
@VideoId varchar(50)
AS
	SELECT [VideoId], [AlbumId], [ImageId], [CreationDate], [Title], [Description], [SortOrder]
	FROM [dbo].[Video]
	WHERE [VideoId]=@VideoId