CREATE TABLE [dbo].[Video]
(
	[VideoId] varchar(50) NOT NULL,
	[AlbumId] int NOT NULL, 
	[ImageId] VARCHAR(30) NULL, 
    [CreationDate] DATETIME NOT NULL,
	[Title]       NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [SortOrder]   INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Video] PRIMARY KEY NONCLUSTERED ([VideoId] ASC),
    CONSTRAINT [FK_Video_VideoAlbum] FOREIGN KEY ([AlbumId]) REFERENCES [VideoAlbum]([AlbumId]) ON DELETE CASCADE,
	CONSTRAINT [FK_Video_Image] FOREIGN KEY ([ImageId]) REFERENCES [Image]([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL
)

GO

CREATE INDEX [IX_Video_CreationDate] ON [dbo].[Video] ([CreationDate] DESC)
