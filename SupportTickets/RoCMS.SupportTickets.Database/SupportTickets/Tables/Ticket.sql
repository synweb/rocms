CREATE TABLE [SupportTickets].[Ticket]
(
	[TicketId] INT NOT NULL IDENTITY(1,1),
	[Subject] NVARCHAR(300) NOT NULL,
	[AuthorId] INT NOT NULL,
	[TicketType] VARCHAR(50) NOT NULL, 
	[Resolved] BIT NOT NULL DEFAULT 0, 
	[Deleted] BIT NOT NULL DEFAULT 0, 
	[CreationDate] DATETIME NOT NULL,
    CONSTRAINT [PK_SupportTicket] PRIMARY KEY ([TicketId]), 
    CONSTRAINT [FK_SupportTicket_User] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User]([UserId]) ON DELETE CASCADE

)