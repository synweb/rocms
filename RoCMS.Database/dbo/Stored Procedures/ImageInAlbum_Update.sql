CREATE PROCEDURE [dbo].[ImageInAlbum_Update]
@AlbumId int,
@ImageId varchar(30),
@Title nvarchar(MAX),
@Description nvarchar(MAX),
@SortOrder int,
@DestinationUrl nvarchar(250)
AS
	UPDATE [ImageInAlbum] SET
		[Title]=@Title,
		[Description]=@Description,
		[SortOrder]=@SortOrder,
		[DestinationUrl]=@DestinationUrl
		WHERE [AlbumId]=@AlbumId AND [ImageId]=@ImageId
			