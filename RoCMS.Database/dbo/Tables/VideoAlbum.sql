CREATE TABLE [dbo].[VideoAlbum]
(
	[AlbumId] INT IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
	[CreationDate] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[OwnerId] INT NULL,
    CONSTRAINT [PK_VideoAlbum] PRIMARY KEY CLUSTERED ([AlbumId] ASC),
    CONSTRAINT [FK_VideoAlbum_User] FOREIGN KEY ([OwnerId]) REFERENCES [User]([UserId])
)
