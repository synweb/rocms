CREATE PROCEDURE [News].[NewsItem_Select]
AS
	SELECT *
	 FROM [News].[NewsItem]
		--WHERE [PostingDate]<=GETUTCDATE()
		ORDER BY [PostingDate] DESC
