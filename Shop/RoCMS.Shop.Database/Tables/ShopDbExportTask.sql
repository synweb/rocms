CREATE TABLE [Shop].[ShopDbExportTask]
(
	[TaskId] INT NOT NULL  IDENTITY, 
    [StartDate] DATETIME NOT NULL, 
    [EndDate] DATETIME NULL, 
    [Status] VARCHAR(25) NOT NULL, 
    [ErrorCode] NVARCHAR(100) NULL, 
    PRIMARY KEY ([TaskId] DESC)
)
