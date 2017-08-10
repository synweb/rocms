CREATE PROCEDURE [dbo].[CmsResource_SelectOne]
	@CmsResourceId int
AS
	SELECT [CmsResourceId], [Name], [Description] FROM [dbo].[CmsResource]
		WHERE [CmsResourceId]=@CmsResourceId
