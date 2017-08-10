CREATE PROCEDURE [News].[Blog_User_SelectByUser]
	@UserId int
AS
	SELECT * FROM [News].[Blog_User] WHERE UserId=@UserId