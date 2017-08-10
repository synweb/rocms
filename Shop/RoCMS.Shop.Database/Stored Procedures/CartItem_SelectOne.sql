CREATE PROCEDURE [Shop].[CartItem_SelectOne]
@CartItemId int
AS
	SELECT * FROM [Shop].[CartItem]
	WHERE [CartItemId]=@CartItemId