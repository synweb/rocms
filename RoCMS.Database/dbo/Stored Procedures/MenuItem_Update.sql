CREATE PROCEDURE [dbo].[MenuItem_Update]
	@MenuItemId int,
	@Name NVARCHAR (200),
    @MenuId  INT,
    @ParentMenuItemId INT,
    @PageUrl NVARCHAR(300),
    @SortOrder INT,
    @BlockId INT
AS

UPDATE MenuItem
SET Name=@Name,
	MenuId=@MenuId,
	ParentMenuItemId=@ParentMenuItemId,
	PageUrl=@PageUrl,
	SortOrder=@SortOrder,
	BlockId=@BlockId
WHERE MenuItemId=@MenuItemId