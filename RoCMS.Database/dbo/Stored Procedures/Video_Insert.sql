CREATE PROCEDURE [dbo].[Video_Insert]
@VideoId varchar(50),
@AlbumId int,
/*@ImageId VARCHAR(30), */
@CreationDate DATETIME,
@Title       NVARCHAR (MAX),
@Description NVARCHAR (MAX)
/*,@SortOrder   INT*/
AS
	INSERT INTO [dbo].[Video] ([VideoId], [AlbumId], [CreationDate], [Title], [Description])
	VALUES (@VideoId, @AlbumId, @CreationDate, @Title, @Description)
