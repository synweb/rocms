CREATE PROCEDURE [Comments].[Comment_SelectOne]
	@CommentId INT
AS
	SELECT * FROM [Comments].[Comment] WHERE [CommentId] = @CommentId
