CREATE PROCEDURE [Shop].[GoodsInOrder_Insert]
@Quantity int,
@GoodsId int,
@OrderId int,
@PackId int,
@Price decimal
AS
	INSERT INTO [Shop].[GoodsInOrder] ([Quantity], [GoodsId], [OrderId], [PackId], [Price])
	VALUES (@Quantity, @GoodsId, @OrderId, @PackId, @Price)
	SELECT @@IDENTITY
