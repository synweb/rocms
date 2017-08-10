CREATE PROCEDURE [Comments].[CommentTopic_Insert]
	@TargetType VARCHAR(50),
	@TargetId INT,
	@TargetUrl NVARCHAR(2000),
	@TargetTitle NVARCHAR(300)
AS
	INSERT INTO [Comments].[CommentTopic] VALUES (@TargetType, @TargetId, @TargetUrl, @TargetTitle)
	SELECT @@IDENTITY
