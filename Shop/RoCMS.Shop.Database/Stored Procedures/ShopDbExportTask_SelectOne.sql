CREATE PROCEDURE [Shop].[ShopDbExportTask_SelectOne]
@TaskId int
AS
	SELECT * FROM [Shop].[ShopDbExportTask] 
	WHERE [TaskId]=@TaskId
