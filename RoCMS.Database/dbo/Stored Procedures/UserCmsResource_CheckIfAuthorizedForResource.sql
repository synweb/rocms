CREATE PROCEDURE [dbo].[UserCmsResource_CheckIfAuthorizedForResource]
	@UserId int,
	@ResourceName varchar(50)
AS
	SELECT COUNT(*) FROM [UserCmsResource] ucr JOIN [CmsResource] cr ON ucr.CmsResourceId=cr.CmsResourceId 
		WHERE ucr.UserId=@UserId AND cr.Name=@ResourceName
