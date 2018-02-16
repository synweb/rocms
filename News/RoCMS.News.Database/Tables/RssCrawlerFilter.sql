CREATE TABLE [News].[RssCrawlerFilter]
(
	[RssCrawlerFilterId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RssCrawlerId] INT NOT NULL,
	[Filter] NVARCHAR(max) NOT NULL, 
    CONSTRAINT [FK_RssCrawlerFilter_Crawler] FOREIGN KEY ([RssCrawlerId]) REFERENCES [News].[RssCrawler]([RssCrawlerId]) ON DELETE CASCADE,
)
