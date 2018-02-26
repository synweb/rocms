CREATE TABLE [News].[RssCrawler]
(
	[RssCrawlerId] INT NOT NULL PRIMARY KEY IDENTITY,
	[RssFeedUrl] nvarchar(max) NOT NULL,
	[IsEnabled] BIT DEFAULT 1 NOT NULL,
	[CheckInterval] INT DEFAULT 720 NOT NULL, -- in minutes
	[TargetCategoryId] INT NULL, 
	[ImageSelector] nvarchar(max) NULL,
	[ContentContainerSelector] nvarchar(max) NULL,
	[LinkText] nvarchar(max) NULL,
	[Tags] nvarchar(max) NULL,
    CONSTRAINT [FK_RssCrawler_Category] FOREIGN KEY ([TargetCategoryId]) REFERENCES [News].[Category]([CategoryId]) ON DELETE SET NULL, 
    CONSTRAINT [CK_RssCrawler_Interval] CHECK (CheckInterval > 0)

)
