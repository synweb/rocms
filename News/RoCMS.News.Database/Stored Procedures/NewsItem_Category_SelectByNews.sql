CREATE PROCEDURE [News].[NewsItem_Category_SelectByNews]
	@NewsId int
AS
	SELECT CategoryId FROM [News].[NewsItem_Category]
		WHERE NewsItemId=@NewsId
