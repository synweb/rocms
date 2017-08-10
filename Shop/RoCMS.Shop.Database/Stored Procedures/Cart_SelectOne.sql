CREATE PROCEDURE [Shop].[Cart_SelectOne]
@CartId uniqueidentifier
AS
	SELECT * FROM [Shop].[Cart]
	WHERE [CartId]=@CartId