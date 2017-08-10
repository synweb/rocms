CREATE PROCEDURE [Shop].[Order_Delete]
@OrderId int
AS
	DELETE FROM [Shop].[Order]
	WHERE [OrderId]=@OrderId
