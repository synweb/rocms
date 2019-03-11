CREATE PROCEDURE [dbo].[FormRequest_Insert]
@Name NVARCHAR(50), 
@Email NVARCHAR(50), 
@Phone NVARCHAR(50), 
@Text NVARCHAR(MAX),
@Amount DECIMAL(18,2),
@State VARCHAR(50),
@PaymentState VARCHAR(50),
@PaymentType VARCHAR(50)

AS
	INSERT INTO [dbo].[FormRequest] ([Name], [Email], [Phone], [Text], [State], [Amount], [PaymentState], [PaymentType])
	VALUES (@Name, @Email, @Phone, @Text, @State, @Amount, @PaymentState, @PaymentType)
	SELECT @@IDENTITY
