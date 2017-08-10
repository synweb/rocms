CREATE PROCEDURE [News].[NewsItem_SelectIdsByTagName]
	@TagName NVARCHAR(200),
	@OnlyPosted bit
AS
		SELECT ni.NewsId
		FROM [News].[NewsItem] ni join [News].[NewsItemTag] nit ON ni.NewsId= nit.NewsItemId
			join [News].[Tag] t ON  nit.TagId = t.TagId
		WHERE t.Name = @TagName AND
			(@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE())
		ORDER BY [PostingDate] DESC
