CREATE PROCEDURE [dbo].[CmsResource_SelectByName]
	@Name varchar(50)
AS
	SELECT * FROM [CmsResource] WHERE [Name]=@Name
