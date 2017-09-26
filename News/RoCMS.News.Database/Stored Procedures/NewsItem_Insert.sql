CREATE PROCEDURE [News].[NewsItem_Insert]
@HeartId int,
@Text nvarchar(MAX),
@PostingDate datetime,
@Description nvarchar(MAX),
@AuthorId int,
@ImageId varchar(30),
@CommentTopicId int,
@RecordType varchar(20),
@Filename NVARCHAR(200),
@VideoId varchar(50),
@BlogId int,
@EventDate datetime
AS
	INSERT INTO [News].[NewsItem] ([HeartId], [Text], [PostingDate], [Description], [AuthorId], [ImageId],  [CommentTopicId], [RecordType]
	, [Filename], [VideoId], [BlogId], [EventDate])
	VALUES (@HeartId, @Text, @PostingDate, @Description, @AuthorId, @ImageId,  
	@CommentTopicId, @RecordType, @Filename, @VideoId, @BlogId, @EventDate)
	SELECT @HeartId
