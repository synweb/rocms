CREATE PROCEDURE [dbo].[MenuItem_Update]
	@MenuItemId int,
	@Name NVARCHAR (200),
    @MenuId  INT,
    @ParentMenuItemId INT,
    @HeartId INT,
    @SortOrder INT,
    @BlockId INT
AS

UPDATE MenuItem
SET Name=@Name,
	MenuId=@MenuId,
	ParentMenuItemId=@ParentMenuItemId,
	HeartId=@HeartId,
	SortOrder=@SortOrder,
	BlockId=@BlockId
WHERE MenuItemId=@MenuItemId