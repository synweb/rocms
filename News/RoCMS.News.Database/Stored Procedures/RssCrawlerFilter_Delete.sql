CREATE PROCEDURE [News].[RssCrawlerFilter_Delete]
@RssCrawlerFilterId int
AS
	DELETE FROM [News].[RssCrawlerFilter]
	WHERE [RssCrawlerFilterId]=@RssCrawlerFilterId
