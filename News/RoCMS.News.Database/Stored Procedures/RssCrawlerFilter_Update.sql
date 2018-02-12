CREATE PROCEDURE [News].[RssCrawlerFilter_Update]
@RssCrawlerId int,
@Filter nvarchar(MAX),
@RssCrawlerFilterId int
AS
	UPDATE [News].[RssCrawlerFilter] SET
		[RssCrawlerId]=@RssCrawlerId,
		[Filter]=@Filter
	WHERE [RssCrawlerFilterId]=@RssCrawlerFilterId
