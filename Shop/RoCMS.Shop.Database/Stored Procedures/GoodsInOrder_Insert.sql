CREATE PROCEDURE [Shop].[GoodsInOrder_Insert]
@Quantity int,
@HeartId int,
@OrderId int,
@PackId int,
@Price decimal
AS
	INSERT INTO [Shop].[GoodsInOrder] ([Quantity], [HeartId], [OrderId], [PackId], [Price])
	VALUES (@Quantity, @HeartId, @OrderId, @PackId, @Price)
	SELECT @@IDENTITY
