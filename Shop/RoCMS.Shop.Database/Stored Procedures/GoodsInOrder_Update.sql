CREATE PROCEDURE [Shop].[GoodsInOrder_Update]
@Quantity int,
@GoodsId int,
@OrderId int,
@PackId int,
@Price decimal,
@Id int
AS
	UPDATE [Shop].[GoodsInOrder] SET
		[Quantity]=@Quantity,
		[GoodsId]=@GoodsId,
		[OrderId]=@OrderId,
		[PackId]=@PackId,
		[Price]=@Price
	WHERE [Id]=@Id
