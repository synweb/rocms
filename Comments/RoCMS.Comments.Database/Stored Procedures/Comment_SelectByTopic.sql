CREATE PROCEDURE [Comments].[Comment_SelectByTopic]
	@CommentTopicId int,
	@Moderated BIT = NULL
AS
	IF @Moderated IS NULL
		SELECT * FROM [Comments].[Comment] WHERE [CommentTopicId]=@CommentTopicId
			ORDER BY [CreationDate] DESC
	ELSE
		SELECT * FROM [Comments].[Comment] WHERE [CommentTopicId]=@CommentTopicId AND [Moderated]=@Moderated
			ORDER BY [CreationDate] DESC