﻿CREATE TABLE [Shop].[GoodsAwaiting]
(
	[GoodsAwaitingId] INT NOT NULL PRIMARY KEY IDENTITY, 
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [HeartId] INT NOT NULL, 
    [Contact] NVARCHAR(50) NOT NULL, 
    [ContactType] VARCHAR(20) NOT NULL, 
    [UserId] INT NULL, 
    [Sent] BIT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_Awaiting_GoodsSet] FOREIGN KEY ([HeartId]) REFERENCES [Shop].[GoodsItem] ([HeartId]) ON DELETE CASCADE,
	CONSTRAINT [FK_Awaiting_UsersSet] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE, 
    CONSTRAINT [AK_GoodsAwaiting_GoodsId_Contact] UNIQUE ([HeartId], [Contact]) 
)
