CREATE TABLE [News].[NewsItemTag]
(
	[NewsItemId] INT NOT NULL,
	[TagId] INT NOT NULL, 
    CONSTRAINT [PK_NewsTag] PRIMARY KEY ([NewsItemId], [TagId]), 
    CONSTRAINT [FK_NewsTag_News] FOREIGN KEY ([NewsItemId]) REFERENCES [News].[NewsItem]([NewsId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_NewsItemTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [News].[Tag]([TagId]) ON DELETE CASCADE,
)
