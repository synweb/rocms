CREATE PROCEDURE [dbo].[Menu_Update]
	@MenuId int,
	@Name nvarchar(200)
AS
	UPDATE Menu
	SET Name = @Name
	WHERE MenuId = @MenuId
