CREATE PROCEDURE [Shop].[ShopDbExportTask_Update]
@StartDate datetime,
@EndDate datetime,
@Status varchar(25),
@ErrorCode nvarchar(100),
@TaskId int
AS
	UPDATE [Shop].[ShopDbExportTask] SET
		[StartDate]=@StartDate,
		[EndDate]=@EndDate,
		[Status]=@Status,
		[ErrorCode]=@ErrorCode
	WHERE [TaskId]=@TaskId
