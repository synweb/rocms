CREATE PROCEDURE [News].[Category_Update]
@Name nvarchar(MAX),
@ParentCategoryId int,
@SortOrder int,
@Hidden bit,
@CategoryId int,
@RelativeUrl nvarchar(300)
AS
	UPDATE [News].[Category] SET
		[Name]=@Name,
		[ParentCategoryId]=@ParentCategoryId,
		[SortOrder]=@SortOrder,
		[Hidden]=@Hidden,
		[RelativeUrl]=@RelativeUrl
	WHERE [CategoryId]=@CategoryId
