CREATE PROCEDURE [dbo].[MenuItem_Delete]
	@MenuItemId int

	AS

	DELETE FROM MenuItem
	WHERE MenuItemId=@MenuItemId
