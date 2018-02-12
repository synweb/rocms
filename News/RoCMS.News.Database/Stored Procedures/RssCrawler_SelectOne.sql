CREATE PROCEDURE [News].[RssCrawler_SelectOne]
@RssCrawlerId int
AS
	SELECT * FROM [News].[RssCrawler]
	WHERE [RssCrawlerId]=@RssCrawlerId
