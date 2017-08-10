CREATE TABLE [FAQ].[Question]
(
	[QuestionId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[QuestionText] NVARCHAR(MAX) NOT NULL,
	[AuthorId] INT NULL,
	[AuthorName] NVARCHAR(200) NULL,
	[AuthorEmail] NVARCHAR(200) NULL,
	[RespondentId] INT NULL,
	[AnswerText] NVARCHAR(MAX) NULL,
	[AnswerSentToAuthor] BIT NOT NULL DEFAULT 0,
	[Moderated] BIT NOT NULL DEFAULT 0, 
	[SortOrder] INT NOT NULL DEFAULT 0,
    CONSTRAINT [FK_Question_Author] FOREIGN KEY ([AuthorId]) REFERENCES [User]([UserId]),
    CONSTRAINT [FK_Question_Respondent] FOREIGN KEY ([RespondentId]) REFERENCES [User]([UserId]),
)

GO

CREATE INDEX [IX_Question_Sort] ON [FAQ].[Question] ([SortOrder])
