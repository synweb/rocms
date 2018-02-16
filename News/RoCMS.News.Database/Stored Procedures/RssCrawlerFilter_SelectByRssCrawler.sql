CREATE PROCEDURE [News].[RssCrawlerFilter_SelectByRssCrawler]
	@RssCrawlerId int
AS
	SELECT * FROM [News].[RssCrawlerFilter]
	WHERE [RssCrawlerId]=@RssCrawlerId

