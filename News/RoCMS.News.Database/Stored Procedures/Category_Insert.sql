CREATE PROCEDURE [News].[Category_Insert]
@Name nvarchar(MAX),
@ParentCategoryId int,
@SortOrder int,
@Hidden bit,
@RelativeUrl nvarchar(300)
AS
	INSERT INTO [News].[Category] ([Name], [ParentCategoryId], [SortOrder], [Hidden],[RelativeUrl])
	VALUES (@Name, @ParentCategoryId, @SortOrder, @Hidden, @RelativeUrl)
	SELECT @@IDENTITY
