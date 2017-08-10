CREATE PROCEDURE [dbo].[Setting_GetValue]
	@Key NVARCHAR (100)
AS
	SELECT [Value] FROM [Setting] WHERE [Key]=@Key
