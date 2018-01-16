CREATE PROCEDURE [Shop].[Goods_Pack_Insert]
@PackId int,
@HeartId int,
@Discount int,
@Price decimal
AS
	INSERT INTO [Shop].[Goods_Pack] ([PackId], [HeartId], [Discount], [Price])
	VALUES (@PackId, @HeartId, @Discount, @Price)
