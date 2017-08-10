CREATE TABLE [dbo].[ImageInAlbum] (
    [AlbumId]     INT            NOT NULL,
    [ImageId]     VARCHAR(30)   NOT NULL,
    [Title]       NVARCHAR (MAX) NULL,
    [Description] NVARCHAR (MAX) NULL,
    [SortOrder]   INT            DEFAULT 0 NOT NULL,
    [DestinationUrl] NVARCHAR(250) NULL, 
    CONSTRAINT [PK_ImageInAlbum] PRIMARY KEY CLUSTERED ([AlbumId] ASC, [ImageId] ASC),
    CONSTRAINT [FK_ImageInAlbum_AlbumId] FOREIGN KEY ([AlbumId]) REFERENCES [dbo].[Album] ([AlbumId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ImageInAlbum_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE CASCADE
);

