CREATE PROCEDURE [Shop].[Pack_Delete]
@PackId int
AS
	DELETE FROM [Shop].[Pack]
	WHERE [PackId]=@PackId
