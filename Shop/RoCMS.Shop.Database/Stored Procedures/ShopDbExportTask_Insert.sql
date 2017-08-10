CREATE PROCEDURE [Shop].[ShopDbExportTask_Insert]
@StartDate datetime,
@EndDate datetime,
@Status varchar(25),
@ErrorCode nvarchar(100)
AS
	INSERT INTO [Shop].[ShopDbExportTask] ([StartDate], [EndDate], [Status], [ErrorCode])
	VALUES (@StartDate, @EndDate, @Status, @ErrorCode)
	SELECT @@IDENTITY
