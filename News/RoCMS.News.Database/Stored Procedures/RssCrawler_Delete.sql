CREATE PROCEDURE [News].[RssCrawler_Delete]
@RssCrawlerId int
AS
	DELETE FROM [News].[RssCrawler]
	WHERE [RssCrawlerId]=@RssCrawlerId
