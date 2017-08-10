CREATE PROCEDURE [News].[NewsItemTag_SelectOne]
@NewsItemId int,
@TagId int
AS
	SELECT [NewsItemId], [TagId] FROM [News].[NewsItemTag]
	WHERE [NewsItemId]=@NewsItemId
		 AND [TagId]=@TagId
