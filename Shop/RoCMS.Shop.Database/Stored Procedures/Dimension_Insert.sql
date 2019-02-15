CREATE PROCEDURE [Shop].[Dimension_Insert]

@Full nvarchar(50),
@Short nvarchar(5)
AS
	INSERT INTO [Shop].[Dimension] ([Full], [Short])
	VALUES (@Full, @Short)
	SELECT @@IDENTITY
