CREATE TRIGGER [Comments].[CommentTopicDelete]
	ON [Comments].[CommentTopic]
	INSTEAD OF DELETE
	AS
	BEGIN
		DELETE FROM [Comments].[Comment]
			WHERE [CommentTopicId] IN (SELECT deleted.[CommentTopicId] FROM deleted)
		DELETE FROM [Comments].[CommentTopic]
			WHERE [CommentTopicId] IN (SELECT deleted.[CommentTopicId] FROM deleted)
	END
