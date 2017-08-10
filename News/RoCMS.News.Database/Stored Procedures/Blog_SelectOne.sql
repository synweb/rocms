CREATE PROCEDURE [News].[Blog_SelectOne]
	@BlogId int
AS
	SELECT * FROM Blog WHERE BlogId = @BlogId
