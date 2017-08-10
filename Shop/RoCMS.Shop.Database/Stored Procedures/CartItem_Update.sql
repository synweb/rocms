CREATE PROCEDURE [Shop].[CartItem_Update]
@PackId int,
@Quantity int,
@CartItemId int
AS
	UPDATE [Shop].[CartItem] SET
		[PackId]=@PackId,
		[Quantity]=@Quantity
	WHERE [CartItemId]=@CartItemId
