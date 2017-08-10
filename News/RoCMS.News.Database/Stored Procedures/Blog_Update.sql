CREATE PROCEDURE [News].[Blog_Update]
	@BlogId int,
	@OwnerId int,
	@Title nvarchar(500),
	@Subtitle nvarchar(500),
	@RelativeUrl nvarchar(200)
AS
	UPDATE Blog
	SET Title = @Title,
		OwnerId = @OwnerId,
		Subtitle = @Subtitle,
		RelativeUrl = @RelativeUrl
	WHERE @BlogId = BlogId

