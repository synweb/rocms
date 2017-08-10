CREATE TABLE [dbo].[Album] (
    [AlbumId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (50) NOT NULL,
	[Description] NVARCHAR(MAX) NULL,
	[CreationDate] DATETIME NOT NULL DEFAULT(GETUTCDATE()),
	[OwnerId] INT NULL,
    CONSTRAINT [PK_AlbumSet] PRIMARY KEY CLUSTERED ([AlbumId] ASC), 
    CONSTRAINT [FK_AlbumSet_User] FOREIGN KEY ([OwnerId]) REFERENCES [User]([UserId])
);

