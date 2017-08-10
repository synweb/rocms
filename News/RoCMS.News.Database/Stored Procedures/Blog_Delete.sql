CREATE PROCEDURE [News].[Blog_Delete]
	@BlogId int
AS
	DELETE FROM Blog WHERE BlogId = @BlogId
