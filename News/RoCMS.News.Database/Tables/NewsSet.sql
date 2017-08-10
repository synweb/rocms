CREATE TABLE [dbo].[NewsSet] (
    [NewsId]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (MAX) NOT NULL,
    [Text]         NVARCHAR (MAX) NOT NULL,
    [PostingDate]  DATETIME       NOT NULL,
    [Description]  NVARCHAR (MAX) NOT NULL,
    [Keywords]     NVARCHAR (MAX) NULL,
    [CreationDate] DATETIME       NOT NULL,
    [AuthorId]     INT            NOT NULL,
    [ImageId]      VARCHAR(30)   NULL,
	[RelativeUrl]  NVARCHAR (300)  NOT NULL,
	[CommentTopicId] INT NULL,
    CONSTRAINT [PK_NewsSet] PRIMARY KEY CLUSTERED ([NewsId] ASC),
    CONSTRAINT [FK_NewsSetImage] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[ImageSet] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT [FK_NewsSetUser] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[UserSet] ([UserId]),
    CONSTRAINT [FK_NewsSet_CommentTopic] FOREIGN KEY ([CommentTopicId]) REFERENCES [Comments].[CommentTopic]([CommentTopicId]) ON DELETE SET NULL,
	UNIQUE NONCLUSTERED ([RelativeUrl] ASC), 
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_NewsUser]
    ON [dbo].[NewsSet]([AuthorId] ASC);

