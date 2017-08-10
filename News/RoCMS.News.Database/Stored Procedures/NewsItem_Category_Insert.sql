CREATE PROCEDURE [News].[NewsItem_Category_Insert]
@NewsItemId int,
@CategoryId int
AS
	INSERT INTO [News].[NewsItem_Category] ([NewsItemId], [CategoryId])
	VALUES (@NewsItemId, @CategoryId)
