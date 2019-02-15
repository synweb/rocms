CREATE PROCEDURE [Shop].[CompatibleSet_Insert]

@Name nvarchar(50)
AS
	INSERT INTO [Shop].[CompatibleSet] ([Name])
	VALUES (@Name)
	SELECT @@IDENTITY
