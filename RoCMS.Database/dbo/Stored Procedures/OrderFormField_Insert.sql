CREATE PROCEDURE [dbo].[OrderFormField_Insert]
@LabelText nvarchar(100),
@ValueType varchar(20),
@Required bit,
@OrderFormId int,
@SortOrder int
AS
	INSERT INTO [dbo].[OrderFormField] ([LabelText], [ValueType], [Required], [OrderFormId], [SortOrder])
	VALUES (@LabelText, @ValueType, @Required, @OrderFormId, @SortOrder)
	SELECT @@IDENTITY