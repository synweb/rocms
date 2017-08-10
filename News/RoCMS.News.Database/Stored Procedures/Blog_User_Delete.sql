CREATE PROCEDURE [News].[Blog_User_Delete]
	@BlogId int,
	@UserId int
AS
	DELETE FROM Blog_User
	WHERE BlogId = @BlogId AND UserId = @UserId
