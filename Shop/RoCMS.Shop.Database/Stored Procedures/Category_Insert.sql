CREATE PROCEDURE [Shop].[Category_Insert]
@HeartId INT,
@Name nvarchar(MAX),
@ParentCategoryId int,
@Description nvarchar(MAX),
@SortOrder int,
@ImageId varchar(30),
@Hidden bit

AS
	INSERT INTO [Shop].[Category] ([HeartId], [Name], [ParentCategoryId], [Description], [SortOrder], [ImageId], [Hidden])
	VALUES (@HeartId, @Name, @ParentCategoryId, @Description, @SortOrder, @ImageId, @Hidden)
	SELECT @HeartId
