CREATE PROCEDURE [dbo].[InterfaceString_SelectOne]
	@Key varchar(200)
AS
	SELECT * FROM InterfaceString WHERE [Key]=@Key
