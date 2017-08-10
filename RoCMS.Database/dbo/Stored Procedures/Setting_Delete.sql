CREATE PROCEDURE [dbo].[Setting_Delete]
	@Key nvarchar(100)
AS
	DELETE FROM [Setting] WHERE [Key]=@Key
