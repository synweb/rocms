CREATE TABLE [Shop].[FavouriteItem]
(
	[SessionId] UNIQUEIDENTIFIER NOT NULL , 
    [HeartId] INT NOT NULL,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    PRIMARY KEY ([HeartId], [SessionId]),
)
