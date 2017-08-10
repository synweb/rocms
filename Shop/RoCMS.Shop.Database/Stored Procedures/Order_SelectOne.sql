CREATE PROCEDURE [Shop].[Order_SelectOne]
@OrderId int
AS
	SELECT * FROM [Shop].[Order]
	WHERE [OrderId]=@OrderId
