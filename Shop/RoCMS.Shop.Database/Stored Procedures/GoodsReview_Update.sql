CREATE PROCEDURE [Shop].[GoodsReview_Update]
@HeartId int,
@Author nvarchar(70),
@AuthorContact nvarchar(150),
@Rating int,
@Text nvarchar(MAX),
@UserId int,
@Moderated bit,
@GoodsReviewId int
AS
	UPDATE [Shop].[GoodsReview] SET
		[HeartId]=@HeartId,
		[Author]=@Author,
		[AuthorContact]=@AuthorContact,
		[Rating]=@Rating,
		[Text]=@Text,
		[UserId]=@UserId,
		[Moderated]=@Moderated
	WHERE [GoodsReviewId]=@GoodsReviewId
