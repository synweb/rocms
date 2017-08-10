CREATE PROCEDURE [News].[NewsItem_Insert]
@Title nvarchar(MAX),
@Text nvarchar(MAX),
@PostingDate datetime,
@Description nvarchar(MAX),
@MetaDescription nvarchar(MAX),
@Keywords nvarchar(MAX),
@AuthorId int,
@ImageId varchar(30),
@RelativeUrl nvarchar(300),
@CommentTopicId int,
@RecordType varchar(20),
@Filename NVARCHAR(200),
@VideoId varchar(50),
@BlogId int,
@RelatedNewsItemId int,
@EventDate datetime,
@AdditionalHeaders nvarchar(MAX)
AS
	INSERT INTO [News].[NewsItem] ([Title], [Text], [PostingDate], [Description], [MetaDescription], [Keywords], [AuthorId], [ImageId], [RelativeUrl], [CommentTopicId], [RecordType]
	, [Filename], [VideoId], [BlogId], [RelatedNewsItemId], [EventDate], [AdditionalHeaders])
	VALUES (@Title, @Text, @PostingDate, @Description, @MetaDescription, @Keywords, @AuthorId, @ImageId, @RelativeUrl, 
	@CommentTopicId, @RecordType, @Filename, @VideoId, @BlogId, @RelatedNewsItemId, @EventDate, @AdditionalHeaders)
	SELECT @@IDENTITY
