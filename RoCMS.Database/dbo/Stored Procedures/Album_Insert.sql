CREATE PROCEDURE [dbo].[Album_Insert]
@Name nvarchar(50),
@Description nvarchar(MAX),
@OwnerId int
AS
	INSERT INTO [dbo].[Album] ([Name], [Description], [OwnerId])
	VALUES (@Name, @Description, @OwnerId)
	SELECT @@IDENTITY
