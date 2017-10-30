CREATE PROCEDURE [dbo].[MenuItem_Insert]
    @Name NVARCHAR (200),
    @MenuId  INT,
    @ParentMenuItemId INT,
    @HeartId INT,
    @SortOrder INT,
    @BlockId INT

AS

INSERT INTO MenuItem ([Name], [MenuId], [ParentMenuItemId], [HeartId], [SortOrder],[BlockId])
VALUES (@Name, @MenuId, @ParentMenuItemId, @HeartId, @SortOrder, @BlockId)
SELECT @@IDENTITY