CREATE TABLE [Shop].[Client] (
    [ClientId]                 INT            IDENTITY (1, 1) NOT NULL,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Email]                    NVARCHAR (250) NULL,
    [Phone]                    NVARCHAR (50)  NULL,
    [EmailNotificationAllowed] BIT            NOT NULL,
    [SmsNotificationAllowed]   BIT            NOT NULL,
    [UserId]                   INT            NULL,
    [Name]                     NVARCHAR (100) NULL,
    [Comment]                  NVARCHAR (MAX) NULL,
    [LastName] NVARCHAR(100) NULL, 
    [Address] NVARCHAR(MAX) NULL, 
    CONSTRAINT [PK_ClientSet] PRIMARY KEY CLUSTERED ([ClientId] ASC),
    CONSTRAINT [FK_ClientUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE SET NULL
);
GO

