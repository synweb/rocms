CREATE PROCEDURE [dbo].[FormRequest_Insert]
@Name NVARCHAR(50), 
@Email NVARCHAR(50), 
@Phone NVARCHAR(50), 
@Text NVARCHAR(MAX)
AS
	INSERT INTO [dbo].[FormRequest] ([Name], [Email], [Phone], [Text])
	VALUES (@Name, @Email, @Phone, @Text)
	SELECT @@IDENTITY
