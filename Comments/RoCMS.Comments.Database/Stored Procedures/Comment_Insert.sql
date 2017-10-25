CREATE PROCEDURE [Comments].[Comment_Insert]
	@ParentCommentId int,
	@HeartId int,
	@Text NVARCHAR(MAX),
	@Moderated BIT,
	@AuthorId INT,
	@Url nvarchar(200),
	@Name nvarchar(200),
	@Email nvarchar(200)
AS
	INSERT INTO [Comments].[Comment] VALUES(@ParentCommentId, @HeartId,
		@Text, @Moderated, @AuthorId, GETUTCDATE(), 0, @Url, @Email, @Name)
	SELECT @@IDENTITY
