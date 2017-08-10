CREATE PROCEDURE [Shop].[Action_Exists]
	@ActionId int
AS
	IF EXISTS( SELECT * FROM [Shop].[Action] WHERE [ActionId]=@ActionId )
		SELECT 1
	ELSE
		SELECT 0