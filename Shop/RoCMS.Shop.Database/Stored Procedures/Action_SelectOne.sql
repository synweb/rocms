CREATE PROCEDURE [Shop].[Action_SelectOne]
@HeartId int
AS
	SELECT * FROM [Shop].[Action]
	WHERE [HeartId]=@HeartId