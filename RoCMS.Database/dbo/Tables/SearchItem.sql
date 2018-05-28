CREATE TABLE [dbo].[SearchItem]
(
	[SearchItemKey] varchar(50) NOT NULL,
	[EntityName] varchar(200) NOT NULL DEFAULT '',
	[EntityId] nvarchar(100) NOT NULL DEFAULT '',
	[FulltextKey] BIGINT NOT NULL IDENTITY(1,1),
	[SearchContent] nvarchar(max) NOT NULL,
	[Text] nvarchar(500) NOT NULL,
	[Title] nvarchar(200) NOT NULL,
	[Url] nvarchar(500) NULL, -- Obsolete
	[HeartId] int NULL, -- Если есть HeartId, урл строим по нему, если нет - берём урл из [Url]
	[ImageId] VARCHAR(30) NULL, 
	[Weight] INT NOT NULL DEFAULT 1,
	[StrictSearch] BIT NOT NULL DEFAULT 0,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[UpdateDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	CONSTRAINT [PK_SearchItem] PRIMARY KEY CLUSTERED ([SearchItemKey], [EntityName], [EntityId]), 
	CONSTRAINT [FK_SearchItem_Image] FOREIGN KEY ([ImageId]) REFERENCES [Image]([ImageId]) ON DELETE SET NULL, 
	CONSTRAINT [FK_SearchItem_Heart] FOREIGN KEY ([HeartId]) REFERENCES [Heart]([HeartId]) ON DELETE CASCADE, 
	CONSTRAINT [AK_SearchItem_Fulltext] UNIQUE ([FulltextKey]), 

	-- внутри должен быть либо Url, либо HeartId, и не иначе.
	CONSTRAINT [CK_SearchItem_UrlsAndHearts] CHECK (([Url] IS NOT NULL AND [HeartId] IS NULL) OR ([Url] IS NULL AND [HeartId] IS NOT NULL))
)

GO

CREATE FULLTEXT INDEX ON [dbo].[SearchItem] ([SearchContent] LANGUAGE 1049) KEY INDEX [AK_SearchItem_Fulltext] ON [Search_FullTextCatalog] WITH CHANGE_TRACKING AUTO
GO
