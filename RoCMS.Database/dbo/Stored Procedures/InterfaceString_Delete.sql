CREATE PROCEDURE [dbo].[InterfaceString_Delete]
	@Key varchar(200)
AS
	DELETE FROM InterfaceString WHERE [Key]=@Key
