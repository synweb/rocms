CREATE PROCEDURE [News].[NewsItem_Update]
@Text nvarchar(MAX),
@PostingDate datetime,
@Description nvarchar(MAX),
@ImageId varchar(30),
@RecordType varchar(20),
@Filename NVARCHAR(200),
@VideoId varchar(50),
@HeartId int,
@EventDate datetime
AS
	UPDATE [News].[NewsItem] SET
		[Text]=@Text,
		[PostingDate]=@PostingDate,
		[Description]=@Description,
		[ImageId]=@ImageId,
		[RecordType]=@RecordType,
		[Filename]=@Filename,
		[VideoId]=@VideoId,
		[EventDate]=@EventDate
	WHERE [HeartId]=@HeartId
