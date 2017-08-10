CREATE PROCEDURE [Comments].[Comment_UpdateModerated]
	@CommentId int,
	@Moderated BIT
AS
	UPDATE [Comments].[Comment] SET [Moderated]=@Moderated WHERE [CommentId]=@CommentId
