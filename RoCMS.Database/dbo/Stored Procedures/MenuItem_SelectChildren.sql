CREATE PROCEDURE [dbo].[MenuItem_SelectChildren]
	@MenuItemId int

AS

SELECT * FROM MenuItem
WHERE ParentMenuItemId=@MenuItemId
