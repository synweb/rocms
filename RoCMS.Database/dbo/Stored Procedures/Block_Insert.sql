CREATE PROCEDURE [dbo].[Block_Insert]
@Title nvarchar(max),
@Content nvarchar(max),
@Name nvarchar(200)
AS
	INSERT INTO [dbo].[Block] ([Title], [Content], [Name])
	VALUES (@Title, @Content, @Name)
	SELECT @@IDENTITY
