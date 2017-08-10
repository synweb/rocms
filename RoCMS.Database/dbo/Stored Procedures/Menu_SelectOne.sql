CREATE PROCEDURE [dbo].[Menu_SelectOne]
	@MenuId int
AS
	SELECT * FROM Menu WHERE MenuId = @MenuId
