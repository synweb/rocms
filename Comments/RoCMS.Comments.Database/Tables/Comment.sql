CREATE TABLE [Comments].[Comment]
(
	[CommentId] INT NOT NULL IDENTITY(1,1),
	[ParentCommentId] INT NULL, 
	[CommentTopicId] INT NOT NULL, 
	[Text] NVARCHAR(MAX) NOT NULL,
	[Moderated] BIT NOT NULL,
	[AuthorId] INT NULL,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[Deleted] BIT NOT NULL DEFAULT 0,
    [Url] NVARCHAR(200) NULL, 
    [Email] NVARCHAR(200) NULL, 
    [Name] NVARCHAR(200) NULL, 
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([CommentId] DESC), 
    CONSTRAINT [FK_Comment_Author] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User]([UserId]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Comment_CommentTopic] FOREIGN KEY ([CommentTopicId]) REFERENCES [Comments].[CommentTopic]([CommentTopicId]),
    CONSTRAINT [FK_Comment_Comment] FOREIGN KEY ([ParentCommentId]) REFERENCES [Comments].[Comment]([CommentId]),
)

GO

CREATE INDEX [IX_Comment_Date] ON [Comments].[Comment] ([CreationDate] DESC)

GO

CREATE INDEX [IX_Comment_Author] ON [Comments].[Comment] ([AuthorId])
