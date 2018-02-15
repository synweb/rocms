CREATE PROCEDURE [News].[NewsItem_Insert]
@HeartId int,
@Text nvarchar(MAX),
@PostingDate datetime,
@Description nvarchar(MAX),
@AuthorId int,
@ImageId varchar(30),
@RecordType varchar(20),
@Filename NVARCHAR(200),
@VideoId varchar(50),
@BlogId int,
@EventDate datetime,
@RssSource nvarchar(max)
AS
	INSERT INTO [News].[NewsItem] ([HeartId], [Text], [PostingDate], [Description], [AuthorId], [ImageId], [RecordType]
	, [Filename], [VideoId], [BlogId], [EventDate], [RssSource])
	VALUES (@HeartId, @Text, @PostingDate, @Description, @AuthorId, @ImageId,  
	@RecordType, @Filename, @VideoId, @BlogId, @EventDate, @RssSource)
	SELECT @HeartId
