CREATE TABLE [dbo].[PasswordTicket]
(
	[TicketId] INT NOT NULL IDENTITY(1,1), 
	[UserId] INT NOT NULL,
	[Token] uniqueidentifier NOT NULL,
	[Used] BIT NOT NULL DEFAULT 0,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[ExpirationDate] DATETIME NOT NULL,
	[UseDate] DATETIME NULL,
    CONSTRAINT [PK_PasswordTicket] PRIMARY KEY ([TicketId]), 
    CONSTRAINT [FK_PasswordTicket_User] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]) ON DELETE CASCADE, 
    CONSTRAINT [AK_PasswordTicket_Token] UNIQUE ([Token]),

)
