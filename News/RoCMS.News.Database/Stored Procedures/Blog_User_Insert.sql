CREATE PROCEDURE [News].[Blog_User_Insert]
	@BlogId int,
	@UserId int
AS
	INSERT INTO [Blog_User] ([BlogId],[UserId])
	VALUES (@BlogId, @UserId)
