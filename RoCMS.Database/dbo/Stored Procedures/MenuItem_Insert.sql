CREATE PROCEDURE [dbo].[MenuItem_Insert]
    @Name NVARCHAR (200),
    @MenuId  INT,
    @ParentMenuItemId INT,
    @PageUrl NVARCHAR(300),
    @SortOrder INT,
    @BlockId INT

AS

INSERT INTO MenuItem ([Name], [MenuId], [ParentMenuItemId], [PageUrl], [SortOrder],[BlockId])
VALUES (@Name, @MenuId, @ParentMenuItemId, @PageUrl, @SortOrder, @BlockId)
SELECT @@IDENTITY