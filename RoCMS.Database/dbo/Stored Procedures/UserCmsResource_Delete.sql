CREATE PROCEDURE [dbo].[UserCmsResource_Delete]
	@UserId int,
	@CmsResourceId int
AS
	DELETE FROM [UserCmsResource] WHERE [UserId]=@UserId AND [CmsResourceId]=@CmsResourceId