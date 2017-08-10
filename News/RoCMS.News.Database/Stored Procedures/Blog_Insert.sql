CREATE PROCEDURE [News].[Blog_Insert]
	@OwnerId int,
	@Title nvarchar(500),
	@Subtitle nvarchar(500),
	@RelativeUrl nvarchar(200)
AS
	INSERT INTO [News].Blog  ([OwnerId], [Title], [Subtitle], [RelativeUrl])
	VALUES (@OwnerId, @Title, @Subtitle, @RelativeUrl)	
	SELECT @@IDENTITY
