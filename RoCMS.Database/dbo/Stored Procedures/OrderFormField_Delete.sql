CREATE PROCEDURE [dbo].[OrderFormField_Delete]
@OrderFormFieldId int
AS
	DELETE FROM [dbo].[OrderFormField]
	WHERE [OrderFormFieldId]=@OrderFormFieldId