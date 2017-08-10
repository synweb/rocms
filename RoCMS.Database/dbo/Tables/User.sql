CREATE TABLE [dbo].[User] (
    [UserId]   INT           IDENTITY (1, 1) NOT NULL,
	[CreationDate] DATETIME DEFAULT GETUTCDATE() NOT NULL,
    [Username] NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (200) NOT NULL,
    [Email] NVARCHAR(100) NULL, 
    [EmailConfirmed] BIT NOT NULL DEFAULT 0, 
    [Description] NVARCHAR(1000) NULL, 
    [Vk] NVARCHAR(100) NULL, 
    [Fb] NVARCHAR(100) NULL, 
    [GoogleP] NVARCHAR(100) NULL, 
    [Twitter] NVARCHAR(100) NULL, 
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [AK_User] UNIQUE NONCLUSTERED ([Username] ASC)
);

