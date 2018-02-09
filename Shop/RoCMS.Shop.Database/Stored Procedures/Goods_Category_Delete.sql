CREATE PROCEDURE [Shop].[Goods_Category_Delete]
@GoodsId int,
@CategoryId int
AS
	DELETE FROM [Shop].[Goods_Category]
	WHERE [GoodsId]=@GoodsId
		 AND [CategoryId]=@CategoryId
