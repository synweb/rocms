CREATE PROCEDURE [Comments].[CommentTopic_Delete]
	@CommentTopicId int
AS
	DELETE FROM [Comments].[CommentTopic] WHERE [CommentTopicId]=@CommentTopicId
