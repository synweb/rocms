CREATE TABLE [dbo].[Mail]
(
	[MailId] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [CreationDate] DATETIME NOT NULL DEFAULT (GETUTCDATE()), 
    [Body] NVARCHAR(MAX) NOT NULL, 
    [Subject] NVARCHAR(MAX) NULL, 
    [Receiver] NVARCHAR(MAX) NOT NULL, 
    [Sent] BIT NOT NULL , 
    [ErrorMessage] NVARCHAR(MAX) NULL, 
    [Attaches] NVARCHAR(MAX) NULL
)
