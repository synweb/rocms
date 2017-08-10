CREATE PROCEDURE [News].[Blog_User_SelectByBlog]
	@BlogId int
AS
	SELECT * FROM [News].[Blog_User] WHERE BlogId=@BlogId
