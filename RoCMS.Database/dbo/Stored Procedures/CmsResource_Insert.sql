CREATE PROCEDURE [dbo].[CmsResource_Insert]
@Name varchar(50),
@Description nvarchar(200)
AS
	INSERT INTO [dbo].[CmsResource] ([Name], [Description])
	VALUES (@Name, @Description)
	SELECT @@IDENTITY
