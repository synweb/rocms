CREATE PROCEDURE [dbo].[ImageInAlbum_Insert]
@AlbumId int,
@ImageId varchar(30),
@Title nvarchar(MAX),
@Description nvarchar(MAX),
@SortOrder int,
@DestinationUrl nvarchar(250)
AS
	INSERT INTO [dbo].[ImageInAlbum] ([AlbumId], [ImageId], [Title], [Description], [SortOrder], [DestinationUrl])
	VALUES (@AlbumId, @ImageId, @Title, @Description, @SortOrder, @DestinationUrl)
