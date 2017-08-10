CREATE PROCEDURE [Comments].[Comment_Delete]
	@CommentId int
AS
	--DELETE FROM [Comments].[Comment] WHERE [CommentId]=@CommentId
	UPDATE [Comments].[Comment] SET [Deleted]=1 WHERE [CommentId]=@CommentId
