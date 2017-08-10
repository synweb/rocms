CREATE PROCEDURE [News].[NewsItem_Category_Delete]
@NewsItemId int,
@CategoryId int
AS
	DELETE FROM [News].[NewsItem_Category]
	WHERE [NewsItemId]=@NewsItemId
		 AND [CategoryId]=@CategoryId
