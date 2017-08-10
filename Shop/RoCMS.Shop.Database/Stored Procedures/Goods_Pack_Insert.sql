CREATE PROCEDURE [Shop].[Goods_Pack_Insert]
@PackId int,
@GoodsId int,
@Discount int,
@Price decimal
AS
	INSERT INTO [Shop].[Goods_Pack] ([PackId], [GoodsId], [Discount], [Price])
	VALUES (@PackId, @GoodsId, @Discount, @Price)
