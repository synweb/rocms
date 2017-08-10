CREATE PROCEDURE [News].[Category_Select]
@ParentId int
AS
	SELECT * FROM [News].[Category]
		WHERE @ParentId IS NULL AND [ParentCategoryId] IS NULL 
		OR [ParentCategoryId]=@ParentId
