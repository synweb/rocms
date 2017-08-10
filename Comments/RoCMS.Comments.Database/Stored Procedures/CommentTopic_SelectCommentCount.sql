CREATE PROCEDURE [Comments].[CommentTopic_SelectCommentCount]
	@CommentTopicId int
AS
	SELECT COUNT(*) FROM [Comments].[Comment] WHERE [CommentTopicId]=@CommentTopicId
