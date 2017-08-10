CREATE PROCEDURE [News].[NewsItem_Category_SelectByCategory]
	@CategoryId int
AS
	SELECT NewsItemId FROM [News].[NewsItem_Category]
		WHERE [CategoryId]=@CategoryId
