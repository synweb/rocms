CREATE PROCEDURE [Shop].[GoodsReview_Insert]
@GoodsId int,
@Author nvarchar(70),
@AuthorContact nvarchar(150),
@Rating int,
@Text nvarchar(MAX),
@UserId int,
@Moderated bit
AS
	INSERT INTO [Shop].[GoodsReview] ([GoodsId], [Author], [AuthorContact], [Rating], [Text], [UserId], [Moderated])
	VALUES (@GoodsId, @Author, @AuthorContact, @Rating, @Text, @UserId, @Moderated)
	SELECT @@IDENTITY
