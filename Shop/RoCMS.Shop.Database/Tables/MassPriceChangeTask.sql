CREATE TABLE [Shop].[MassPriceChangeTask]
(
	[TaskId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Description] NVARCHAR(MAX) NULL, 
    [State] VARCHAR(20) NOT NULL, 
    [Comment] NVARCHAR(MAX) NULL, 
    [CreationDate]  DATETIME NOT NULL DEFAULT GETUTCDATE(),
)
