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
    [HeartId]       INT   NOT NULL,
    [Text]         NVARCHAR (MAX) NOT NULL,
    [PostingDate]  DATETIME       NOT NULL,
    [Description]  NVARCHAR (MAX) NOT NULL,
    [AuthorId]     INT            NULL,
    [ImageId]      VARCHAR (30)   NULL,
	[RecordType] VARCHAR(20) NOT NULL DEFAULT 'Default',
	[Filename] NVARCHAR(200) NULL,
	[VideoId] varchar(50) NULL,
    [BlogId] INT NULL, 
    [EventDate] DATETIME NULL, 
	[ViewCount] BIGINT NOT NULL DEFAULT 0,
	[CreationDate] DATETIME NOT NULL,
	[RssSource] nvarchar(max) NULL
	)
	
		INSERT INTO @news ([HeartId],  [Text], [PostingDate], [Description], [AuthorId], [ImageId], [RecordType], [Filename], [VideoId], [BlogId], [EventDate], [ViewCount], [CreationDate], [RssSource] )
	SELECT DISTINCT ni.[HeartId], [Text], [PostingDate], [Description], [AuthorId], [ImageId], [RecordType], [Filename], [VideoId], [BlogId], [EventDate], [ViewCount], h.[CreationDate], [RssSource]
		FROM [News].[NewsItem] ni join [dbo].[Heart] h on ni.HeartId=h.HeartId
			JOIN @NewsIds ids ON ni.[HeartId]=ids.Val
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