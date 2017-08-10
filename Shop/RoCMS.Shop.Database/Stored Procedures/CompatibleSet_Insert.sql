CREATE PROCEDURE [Shop].[CompatibleSet_Insert]
@Guid uniqueidentifier,
@Name nvarchar(50)
AS
	INSERT INTO [Shop].[CompatibleSet] ([Guid], [Name])
	VALUES (@Guid, @Name)
	SELECT @@IDENTITY
