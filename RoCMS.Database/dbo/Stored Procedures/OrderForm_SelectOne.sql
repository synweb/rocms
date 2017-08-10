CREATE PROCEDURE [dbo].[OrderForm_SelectOne]
@OrderFormId int
AS
	SELECT * FROM [dbo].[OrderForm]
	WHERE [OrderFormId]=@OrderFormId

