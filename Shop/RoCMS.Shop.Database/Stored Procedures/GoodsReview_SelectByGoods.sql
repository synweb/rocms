CREATE PROCEDURE [Shop].[GoodsReview_SelectByGoods]
	@GoodsId int
AS
	SELECT * FROM [Shop].GoodsReview WHERE [GoodsId]=@GoodsId
