CREATE PROCEDURE [dbo].[UserCmsResource_Insert]
	@UserId int,
	@CmsResourceId int
AS
	INSERT INTO [UserCmsResource] (UserId, CmsResourceId) VALUES (@UserId, @CmsResourceId)