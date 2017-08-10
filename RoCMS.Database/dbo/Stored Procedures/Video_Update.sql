CREATE PROCEDURE [dbo].[Video_Update]
@VideoId varchar(50),
@AlbumId int,
@ImageId VARCHAR(30),
@CreationDate DATETIME,
@Title       NVARCHAR (MAX),
@Description NVARCHAR (MAX),
@SortOrder   INT
AS
	UPDATE [dbo].[Video]
	SET [AlbumId]=@AlbumId,
		[ImageId]=@ImageId, 
		[CreationDate]=@CreationDate,
		[Title]=@Title, 
		[Description]=@Description, 
		[SortOrder]=@SortOrder
	WHERE [VideoId]=@VideoId
