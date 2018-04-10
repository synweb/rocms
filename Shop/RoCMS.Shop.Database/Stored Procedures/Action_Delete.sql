CREATE PROCEDURE [Shop].[Action_Delete]
@HeartId int
AS
	DELETE FROM [Shop].[Action]
	WHERE [HeartId]=@HeartId
