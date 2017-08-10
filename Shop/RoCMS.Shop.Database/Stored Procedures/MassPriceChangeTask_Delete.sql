CREATE PROCEDURE [Shop].[MassPriceChangeTask_Delete]
@TaskId int
AS
	DELETE FROM [Shop].[MassPriceChangeTask]
	WHERE [TaskId]=@TaskId
