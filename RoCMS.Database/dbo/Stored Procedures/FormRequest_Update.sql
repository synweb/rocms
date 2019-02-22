CREATE PROCEDURE [dbo].[FormRequest_Update]
@FormRequestId INT,
@Name NVARCHAR(50), 
@Email NVARCHAR(50), 
@Phone NVARCHAR(50), 
@Text NVARCHAR(MAX),
@Amount DECIMAL(18,2),
@State VARCHAR(50),
@PaymentState VARCHAR(50),
@PaymentType VARCHAR(50)
AS
	UPDATE [dbo].[FormRequest]
	SET [Name] = @Name,
		[Email] = @Email,
		[Phone] = @Phone,
		[Text] = @Text,
		[Amount] = @Amount,
		[State] = @State,
		[PaymentState]=@PaymentState,
		[PaymentType]=@PaymentType
	WHERE [FormRequestId] = @FormRequestId
