CREATE PROCEDURE [dbo].[OrderForm_Delete]
@OrderFormId int
AS
	DELETE FROM [dbo].[OrderForm]
	WHERE [OrderFormId]=@OrderFormId