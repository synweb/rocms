CREATE TABLE [News].[NewsItem] (
    [HeartId]       INT   NOT NULL,
    [Text]         NVARCHAR (MAX) NOT NULL,
    [PostingDate]  DATETIME       NOT NULL,
    [Description]  NVARCHAR (MAX) NOT NULL,
    [AuthorId]     INT            NOT NULL,
    [ImageId]      VARCHAR (30)   NULL,
	[CommentTopicId] INT NULL,
	[RecordType] VARCHAR(20) NOT NULL DEFAULT 'Default',
	[Filename] NVARCHAR(200) NULL,
	[VideoId] varchar(50) NULL,
    [BlogId] INT NULL, 
    [EventDate] DATETIME NULL, 
	[ViewCount] BIGINT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_NewsSet] PRIMARY KEY CLUSTERED ([HeartId] ASC),
    CONSTRAINT [FK_NewsSetImage] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT [FK_NewsSetUser] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User] ([UserId]),
	CONSTRAINT [FK_NewsSetBlog] FOREIGN KEY ([BlogId]) REFERENCES [News].[Blog] ([BlogId]),
    CONSTRAINT [FK_NewsSet_CommentTopic] FOREIGN KEY ([CommentTopicId]) REFERENCES [Comments].[CommentTopic]([CommentTopicId]) ON DELETE SET NULL,
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_NewsUser]
    ON [News].[NewsItem]([AuthorId] ASC);

