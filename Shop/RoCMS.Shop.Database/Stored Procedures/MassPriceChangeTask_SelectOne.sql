CREATE PROCEDURE [Shop].[MassPriceChangeTask_SelectOne]
@TaskId int
AS
	SELECT * FROM [Shop].[MassPriceChangeTask]
	WHERE [TaskId]=@TaskId
