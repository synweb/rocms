CREATE PROCEDURE [Comments].[CommentTopic_SelectOne]
	@CommentTopicId int
AS
	SELECT * FROM [Comments].[CommentTopic] WHERE [CommentTopicId]=@CommentTopicId
