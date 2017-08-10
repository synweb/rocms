CREATE PROCEDURE [Shop].[Action_Delete]
@ActionId int
AS
	DELETE FROM [Shop].[Action]
	WHERE [ActionId]=@ActionId
