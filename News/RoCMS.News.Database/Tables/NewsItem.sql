CREATE TABLE [News].[NewsItem] (
    [NewsId]       INT            IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (MAX) NOT NULL,
    [Text]         NVARCHAR (MAX) NOT NULL,
    [PostingDate]  DATETIME       NOT NULL,
    [Description]  NVARCHAR (MAX) NOT NULL,
	[MetaDescription]  NVARCHAR (MAX) NULL,
    [Keywords]     NVARCHAR (MAX) NULL,
    [CreationDate] DATETIME       NOT NULL DEFAULT GETUTCDATE(),
    [AuthorId]     INT            NOT NULL,
    [ImageId]      VARCHAR (30)   NULL,
	[RelativeUrl]  NVARCHAR (300)  NOT NULL,
	[CommentTopicId] INT NULL,
	[RecordType] VARCHAR(20) NOT NULL DEFAULT 'Default',
	[Filename] NVARCHAR(200) NULL,
	[VideoId] varchar(50) NULL,
    [BlogId] INT NULL, 
    [RelatedNewsItemId] INT NULL, 
    [EventDate] DATETIME NULL, 
	[AdditionalHeaders] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_NewsSet] PRIMARY KEY CLUSTERED ([NewsId] ASC),
    CONSTRAINT [FK_NewsSetImage] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT [FK_NewsSetUser] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User] ([UserId]),
	CONSTRAINT [FK_NewsSetBlog] FOREIGN KEY ([BlogId]) REFERENCES [News].[Blog] ([BlogId]),
    CONSTRAINT [FK_NewsSet_CommentTopic] FOREIGN KEY ([CommentTopicId]) REFERENCES [Comments].[CommentTopic]([CommentTopicId]) ON DELETE SET NULL,
	UNIQUE NONCLUSTERED ([RelativeUrl] ASC), 
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_NewsUser]
    ON [News].[NewsItem]([AuthorId] ASC);

