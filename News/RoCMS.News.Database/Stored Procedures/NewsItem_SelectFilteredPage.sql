CREATE PROCEDURE [News].[NewsItem_SelectFilteredPage]
	@NewsIds Int_Table READONLY,
	@PageNumber int,
	@PageSize int,
	@TotalCount int OUT,
	@OnlyFutureEventDate bit,
	@OnlyPosted bit,
	@BlogId int,
	@RecordTypes String_Table READONLY,

	@SortBy VARCHAR(20),
	@SortOrder VARCHAR(4)

AS

	DECLARE @news TABLE(
	[NewsId]       INT             NOT NULL,
	[Title]        NVARCHAR (MAX) NOT NULL,
	[Text]         NVARCHAR (MAX) NOT NULL,
	[PostingDate]  DATETIME       NOT NULL,
	[Description]  NVARCHAR (MAX) NOT NULL,
	[MetaDescription]  NVARCHAR (MAX) NULL,
	[Keywords]     NVARCHAR (MAX) NULL,
	[CreationDate] DATETIME       NOT NULL,
	[AuthorId]     INT            NOT NULL,
	[ImageId]      VARCHAR (30)   NULL,
	[RelativeUrl]  NVARCHAR (300)  NOT NULL,
	[CommentTopicId] INT NULL,	
	[RecordType] VARCHAR(20) NULL,
	[Filename] NVARCHAR(200) NULL,
	[VideoId] varchar(50) NULL,
	[BlogId] INT NULL,
	[EventDate]  DATETIME NULL,
	[AdditionalHeaders] NVARCHAR (MAX) NULL,
	[ViewCount] BIGINT NOT NULL DEFAULT 0
	)
	
		INSERT INTO @news ([NewsId], [Title], [Text], [PostingDate], [Description], [MetaDescription], [Keywords], [CreationDate], [AuthorId], [ImageId], [RelativeUrl], [CommentTopicId],[RecordType], [Filename], [VideoId], [BlogId], [EventDate], [AdditionalHeaders], [ViewCount] )
	SELECT DISTINCT ni.[NewsId], [Title], [Text], [PostingDate], [Description], [MetaDescription], [Keywords], ni.[CreationDate], [AuthorId], [ImageId], [RelativeUrl], [CommentTopicId] ,[RecordType], [Filename], [VideoId], [BlogId], [EventDate], [AdditionalHeaders], [ViewCount]
		FROM [News].[NewsItem] ni 
			JOIN @NewsIds ids ON ni.NewsId=ids.Val
		WHERE 
		
		(@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE())
		AND
		(
			@OnlyFutureEventDate IS NULL
			OR
			@OnlyFutureEventDate=0 AND [EventDate]<GETUTCDATE()
			OR
			@OnlyFutureEventDate=1 AND [EventDate]>=GETUTCDATE()
		)
		AND
		(
			NOT EXISTS(SELECT * FROM @RecordTypes)
				OR
			[RecordType] IN (SELECT * FROM @RecordTypes)
		)
		AND
		(@BlogId IS NULL OR [BlogId]=@BlogId)

	SELECT @TotalCount = COUNT(*) FROM @news

IF @SortBy='PostingDate'
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
ELSE
IF @SortBy='CreationDate'
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [CreationDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [CreationDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
ELSE
IF @SortBy='EventDate'
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [EventDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [EventDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
ELSE
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END