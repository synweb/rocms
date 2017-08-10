CREATE PROCEDURE [Shop].[Action_SelectOne]
@ActionId int
AS
	SELECT * FROM [Shop].[Action]
	WHERE [ActionId]=@ActionId