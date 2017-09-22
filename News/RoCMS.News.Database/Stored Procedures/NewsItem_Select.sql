CREATE PROCEDURE [News].[NewsItem_Select]
AS
	SELECT * FROM [News].[NewsItem]
		ORDER BY [PostingDate] DESC
