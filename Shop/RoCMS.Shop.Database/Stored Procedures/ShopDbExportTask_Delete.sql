CREATE PROCEDURE [Shop].[ShopDbExportTask_Delete]
@TaskId int
AS
	DELETE FROM [Shop].[ShopDbExportTask]
	WHERE [TaskId]=@TaskId
