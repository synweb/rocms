CREATE PROCEDURE [Shop].[Cart_Delete]
@CartId uniqueidentifier
AS
	DELETE FROM [Shop].[Cart]
	WHERE [CartId]=@CartId
