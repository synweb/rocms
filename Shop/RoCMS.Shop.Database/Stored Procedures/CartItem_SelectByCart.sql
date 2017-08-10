CREATE PROCEDURE [Shop].[CartItem_SelectByCart]
@CartId UNIQUEIDENTIFIER
AS
	SELECT * FROM [Shop].[CartItem]
		WHERE [CartId]=@CartId