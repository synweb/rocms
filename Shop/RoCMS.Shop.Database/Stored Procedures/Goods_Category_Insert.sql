CREATE PROCEDURE [Shop].[Goods_Category_Insert]
@GoodsId int,
@CategoryId int
AS
	INSERT INTO [Shop].[Goods_Category] ([GoodsId], [CategoryId])
	VALUES (@GoodsId, @CategoryId)
