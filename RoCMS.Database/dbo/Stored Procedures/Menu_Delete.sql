CREATE PROCEDURE [dbo].[Menu_Delete]
	@MenuId int
AS
	DELETE FROM Menu
	WHERE MenuId = @MenuId
