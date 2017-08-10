CREATE PROCEDURE [dbo].[CmsResource_Delete]
@CmsResourceId int
AS
	DELETE FROM [dbo].[CmsResource]
	WHERE [CmsResourceId]=@CmsResourceId
