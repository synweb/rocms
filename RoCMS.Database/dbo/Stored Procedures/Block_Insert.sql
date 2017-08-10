CREATE PROCEDURE [dbo].[Block_Insert]
@Title nvarchar(max),
@Content nvarchar(max)
AS
	INSERT INTO [dbo].[Block] ([Title], [Content])
	VALUES (@Title, @Content)
	SELECT @@IDENTITY
