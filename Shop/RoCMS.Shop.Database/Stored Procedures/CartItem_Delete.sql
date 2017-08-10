CREATE PROCEDURE [Shop].[CartItem_Delete]
@CartItemId int
AS
	DELETE FROM [Shop].[CartItem]
	WHERE [CartItemId]=@CartItemId
