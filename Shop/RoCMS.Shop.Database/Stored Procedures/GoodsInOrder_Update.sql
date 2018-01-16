CREATE PROCEDURE [Shop].[GoodsInOrder_Update]
@Quantity int,
@HeartId int,
@OrderId int,
@PackId int,
@Price decimal,
@Id int
AS
	UPDATE [Shop].[GoodsInOrder] SET
		[Quantity]=@Quantity,
		[HeartId]=@HeartId,
		[OrderId]=@OrderId,
		[PackId]=@PackId,
		[Price]=@Price
	WHERE [Id]=@Id
