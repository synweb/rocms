CREATE PROCEDURE [Shop].[Pack_SelectOne]
@PackId int
AS
	SELECT * FROM [Shop].[Pack]
	WHERE [PackId]=@PackId
