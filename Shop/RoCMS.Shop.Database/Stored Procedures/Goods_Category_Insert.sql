CREATE PROCEDURE [Shop].[Goods_Category_Insert]
@HeartId int,
@CategoryId int
AS
	INSERT INTO [Shop].[Goods_Category] ([GoodsHeartId], [CategoryId])
	VALUES (@HeartId, @CategoryId)
