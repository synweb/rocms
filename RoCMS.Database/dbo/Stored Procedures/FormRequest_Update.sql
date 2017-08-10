CREATE PROCEDURE [dbo].[FormRequest_Update]
@FormRequestId INT,
@Name NVARCHAR(50), 
@Email NVARCHAR(50), 
@Phone NVARCHAR(50), 
@Text NVARCHAR(MAX)
AS
	UPDATE [dbo].[FormRequest]
	SET [Name] = @Name,
		[Email] = @Email,
		[Phone] = @Phone,
		[Text] = @Text
	WHERE [FormRequestId] = @FormRequestId
