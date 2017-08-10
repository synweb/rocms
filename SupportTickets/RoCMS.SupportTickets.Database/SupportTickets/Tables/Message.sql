CREATE TABLE [SupportTickets].[Message]
(
	[MessageId] INT NOT NULL IDENTITY(1,1),
	[AuthorId] INT NOT NULL,
	[TicketId] INT NOT NULL,
	[Text] NVARCHAR(MAX) NOT NULL, 
	[IsRead] BIT NOT NULL DEFAULT 0, 
	[Deleted] BIT NOT NULL DEFAULT 0, 
	[CreationDate] DATETIME NOT NULL, 
    CONSTRAINT [PK_Message] PRIMARY KEY ([MessageId]), 
    CONSTRAINT [FK_Message_Topic] FOREIGN KEY ([TicketId]) REFERENCES [SupportTickets].[Ticket]([TicketId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Message_User] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User]([UserId]),

)
