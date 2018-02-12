CREATE TABLE [News].[RssCrawler]
(
	[RssCrawlerId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RssFeedUrl] nvarchar(max) NOT NULL,
	[IsEnabled] BIT DEFAULT 1 NOT NULL,
	[CheckInterval] INT DEFAULT 720 NOT NULL, -- in minutes
	[TargetCategory] INT NULL, 
    CONSTRAINT [FK_RssCrawler_Category] FOREIGN KEY ([TargetCategory]) REFERENCES [News].[Category]([CategoryId]) ON DELETE SET NULL

)
