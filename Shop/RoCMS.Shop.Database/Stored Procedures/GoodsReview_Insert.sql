CREATE PROCEDURE [Shop].[GoodsReview_Insert]
@HeartId int,
@Author nvarchar(70),
@AuthorContact nvarchar(150),
@Rating int,
@Text nvarchar(MAX),
@UserId int,
@Moderated bit
AS
	INSERT INTO [Shop].[GoodsReview] ([HeartId], [Author], [AuthorContact], [Rating], [Text], [UserId], [Moderated])
	VALUES (@HeartId, @Author, @AuthorContact, @Rating, @Text, @UserId, @Moderated)
	SELECT @@IDENTITY
