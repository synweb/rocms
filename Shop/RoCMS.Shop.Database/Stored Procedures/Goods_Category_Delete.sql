CREATE PROCEDURE [Shop].[Goods_Category_Delete]
@HeartId int,
@CategoryId int
AS
	DELETE FROM [Shop].[Goods_Category]
	WHERE [GoodsHeartId]=@HeartId
		 AND [CategoryId]=@CategoryId
