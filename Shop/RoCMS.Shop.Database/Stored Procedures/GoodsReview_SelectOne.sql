CREATE PROCEDURE [Shop].[GoodsReview_SelectOne]
@GoodsReviewId int
AS
	SELECT * FROM [Shop].[GoodsReview]
	WHERE [GoodsReviewId]=@GoodsReviewId
