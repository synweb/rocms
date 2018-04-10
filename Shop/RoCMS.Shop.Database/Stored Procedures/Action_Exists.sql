CREATE PROCEDURE [Shop].[Action_Exists]
	@HeartId int
AS
	IF EXISTS( SELECT * FROM [Shop].[Action] WHERE [HeartId]=@HeartId )
		SELECT 1
	ELSE
		SELECT 0