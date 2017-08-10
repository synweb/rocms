CREATE PROCEDURE [Comments].[Comment_UpdateText]
	@CommentId int,
	@Text NVARCHAR(MAX)
AS
	UPDATE [Comments].[Comment] SET [Text]=@Text WHERE [CommentId]=@CommentId
