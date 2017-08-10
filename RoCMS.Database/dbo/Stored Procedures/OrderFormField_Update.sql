CREATE PROCEDURE [dbo].[OrderFormField_Update]
@LabelText nvarchar(100),
@ValueType varchar(20),
@Required bit,
@OrderFormId int,
@OrderFormFieldId int,
@SortOrder int
AS
	UPDATE [dbo].[OrderFormField] SET
		[LabelText]=@LabelText,
		[ValueType]=@ValueType,
		[Required]=@Required,
		[OrderFormId]=@OrderFormId,
		[SortOrder]=@SortOrder
	WHERE [OrderFormFieldId]=@OrderFormFieldId