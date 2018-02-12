CREATE PROCEDURE [News].[RssCrawlerFilter_Insert]
@RssCrawlerId int,
@Filter nvarchar(MAX)
AS
	INSERT INTO [News].[RssCrawlerFilter] ([RssCrawlerId], [Filter])
	VALUES (@RssCrawlerId, @Filter)
	SELECT @@IDENTITY
