CREATE PROCEDURE [Shop].[GoodsReview_SelectByGoods]
	@HeartId int
AS
	SELECT * FROM [Shop].GoodsReview WHERE [HeartId]=@HeartId
