CREATE PROCEDURE [Shop].[GoodsReview_Delete]
@GoodsReviewId int
AS
	DELETE FROM [Shop].[GoodsReview]
	WHERE [GoodsReviewId]=@GoodsReviewId
