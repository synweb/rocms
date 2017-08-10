CREATE PROCEDURE [News].[Tag_SelectByNews]
	@NewsId int
AS
	SELECT t.[TagId], [CreationDate], [Name] FROM [News].[Tag] t JOIN [News].[NewsItemTag] nit ON t.TagId = nit.TagId
		WHERE nit.NewsItemId = @NewsId
