CREATE PROCEDURE [dbo].[UserCmsResource_SelectByUser]
	@UserId int
AS
	SELECT * FROM [UserCmsResource] WHERE UserId=@UserId