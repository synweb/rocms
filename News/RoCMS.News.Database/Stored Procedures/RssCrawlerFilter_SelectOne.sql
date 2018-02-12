CREATE PROCEDURE [News].[RssCrawlerFilter_SelectOne]
@RssCrawlerFilterId int
AS
	SELECT * FROM [News].[RssCrawlerFilter]
	WHERE [RssCrawlerFilterId]=@RssCrawlerFilterId
