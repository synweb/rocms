CREATE PROCEDURE [News].[NewsItem_Delete]
@NewsId int
AS
	DELETE FROM [News].[NewsItem]
	WHERE [NewsId]=@NewsId
