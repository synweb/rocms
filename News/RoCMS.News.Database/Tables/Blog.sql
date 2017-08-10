CREATE TABLE [News].[Blog]
(
	[BlogId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Title] NVARCHAR(500) NULL, 
    [Subtitle] NVARCHAR(500) NULL, 
    [OwnerId] INT NULL, 
    [RelativeUrl] NVARCHAR(200) NULL,

	CONSTRAINT [FK_Blog_User] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User]([UserId])
)
GO

CREATE UNIQUE INDEX [IX_Blog_Url] ON [News].[Blog] ([RelativeUrl]) WHERE ([RelativeUrl] IS NOT NULL)
GO