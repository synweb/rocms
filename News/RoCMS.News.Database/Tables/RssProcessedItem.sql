CREATE TABLE [News].[RssProcessedItem]
(
	[RssProcessedItemId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[RssSource] nvarchar(4000) NOT NULL UNIQUE,
	[CreationDate] datetime not null default GETUTCDATE(),
	[NewsItemId] int null, 
    CONSTRAINT [FK_RssProcessedItems_NewsItem] FOREIGN KEY ([NewsItemId]) REFERENCES [News].[NewsItem]([HeartId])
)
