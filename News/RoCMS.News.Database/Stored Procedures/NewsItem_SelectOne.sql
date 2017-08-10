CREATE PROCEDURE [News].[NewsItem_SelectOne]
@NewsId int
AS
	SELECT *
	FROM [News].[NewsItem]
	WHERE [NewsId]=@NewsId
