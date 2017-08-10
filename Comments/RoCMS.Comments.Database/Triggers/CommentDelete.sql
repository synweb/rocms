CREATE TRIGGER [Comments].[CommentDelete]
	ON [Comments].[Comment]
	INSTEAD OF DELETE
	AS
	BEGIN
		DELETE FROM [Comments].[Comment]
			WHERE [ParentCommentId] IN (SELECT deleted.[CommentId] FROM deleted)
		DELETE FROM [Comments].[Comment]
			WHERE [CommentId] IN (SELECT deleted.[CommentId] FROM deleted)
		
	END
