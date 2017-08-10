CREATE PROCEDURE [Shop].[Category_Select]
	@ParentCategoryId int 
AS
	SELECT * FROM [Shop].[Category] WHERE ([ParentCategoryId] IS NULL AND @ParentCategoryId IS NULL) OR [ParentCategoryId]=@ParentCategoryId