CREATE PROCEDURE [News].[NewsItem_SelectByUrl]
	@RelativeUrl nvarchar(300),
	@OnlyPosted bit
AS
		
	SELECT *
		FROM [News].[NewsItem]
	WHERE [RelativeUrl]=@RelativeUrl AND 
		(@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE())
