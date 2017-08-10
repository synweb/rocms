CREATE PROCEDURE [dbo].[CmsResource_Update]
@Name varchar(50),
@Description nvarchar(200),
@CmsResourceId int
AS
	UPDATE [dbo].[CmsResource] SET
		[Name]=@Name,
		[Description]=@Description
	WHERE [CmsResourceId]=@CmsResourceId
