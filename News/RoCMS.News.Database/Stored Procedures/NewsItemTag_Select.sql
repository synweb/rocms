CREATE PROCEDURE [News].[NewsItemTag_Select]
AS
	SELECT [NewsItemId], [TagId] FROM [News].[NewsItemTag]
