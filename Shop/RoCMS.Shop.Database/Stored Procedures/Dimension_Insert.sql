CREATE PROCEDURE [Shop].[Dimension_Insert]
@Guid uniqueidentifier,
@Full nvarchar(50),
@Short nvarchar(5)
AS
	INSERT INTO [Shop].[Dimension] ([Guid], [Full], [Short])
	VALUES (@Guid, @Full, @Short)
	SELECT @@IDENTITY
