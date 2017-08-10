CREATE TABLE [dbo].[SearchItem]
(
	[SearchItemKey] varchar(50) NOT NULL,
	[EntityName] varchar(200) NOT NULL DEFAULT '',
	[EntityId] nvarchar(100) NOT NULL DEFAULT '',
	[FulltextKey] BIGINT NOT NULL IDENTITY(1,1), -- AS [EntityName] + '_' + [SearchItemKey] + '_' +  [EntityId]
	[SearchContent] nvarchar(max) NOT NULL,
	[Text] nvarchar(500) NOT NULL,
	[Title] nvarchar(200) NOT NULL,
	[Url] nvarchar(500) NOT NULL, 
	[ImageId] VARCHAR(30) NULL, 
	[Weight] INT NOT NULL DEFAULT 1,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[UpdateDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_SearchItem] PRIMARY KEY CLUSTERED ([SearchItemKey], [EntityName], [EntityId]), 
    CONSTRAINT [FK_SearchItem_Image] FOREIGN KEY ([ImageId]) REFERENCES [Image]([ImageId]), 
    CONSTRAINT [AK_SearchItem_Fulltext] UNIQUE ([FulltextKey]) 
)

GO

CREATE FULLTEXT INDEX ON [dbo].[SearchItem] ([SearchContent] LANGUAGE 1049) KEY INDEX [AK_SearchItem_Fulltext] ON [Search_FullTextCatalog] WITH CHANGE_TRACKING AUTO
GO
