CREATE PROCEDURE [News].[Tag_Insert]
@Name nvarchar(200)
AS
	INSERT INTO [News].[Tag] ([Name])
	VALUES (@Name)
	SELECT @@IDENTITY
