CREATE PROCEDURE [News].[NewsItemTag_Delete]
@NewsItemId int,
@TagId int
AS
	DELETE FROM [News].[NewsItemTag]
	WHERE [NewsItemId]=@NewsItemId
		 AND [TagId]=@TagId
