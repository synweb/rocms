CREATE TABLE [News].[NewsItem_Category]
(
	[NewsItemId] INT NOT NULL,
	[CategoryId] INT NOT NULL, 
    CONSTRAINT [PK_NewsItem_Category] PRIMARY KEY ([NewsItemId],[CategoryId]), 
    CONSTRAINT [FK_NewsItem_Category_News] FOREIGN KEY ([NewsItemId]) REFERENCES [News].[NewsItem]([HeartId]) ON DELETE CASCADE,
    CONSTRAINT [FK_NewsItem_Category_Category] FOREIGN KEY ([CategoryId]) REFERENCES [News].[Category]([CategoryId]) ON DELETE CASCADE,
)
