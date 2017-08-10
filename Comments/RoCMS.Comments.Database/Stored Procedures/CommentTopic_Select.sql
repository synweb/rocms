CREATE PROCEDURE [Comments].[CommentTopic_Select]
	@Page int,
	@PageSize int,
	@TotalCount int out,
	@TopicType VARCHAR(50) = null

AS
	DECLARE @temp TABLE (
		[CommentTopicId] INT,
		[TargetType] VARCHAR(50),
		[TargetId] INT,
		[TargetUrl] NVARCHAR(2000),
		[TargetTitle] NVARCHAR(300)
		)
	INSERT INTO @temp ([CommentTopicId], [TargetType], [TargetId], [TargetUrl], [TargetTitle])
		(SELECT [CommentTopicId], [TargetType], [TargetId], [TargetUrl], [TargetTitle] FROM [Comments].[CommentTopic])

	SELECT @TotalCount = COUNT(*) FROM @temp
	
	SELECT * FROM @temp
	ORDER BY [CommentTopicId] DESC
	OFFSET (@Page - 1 )*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
