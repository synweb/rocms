CREATE TABLE [dbo].[FormRequest]
(
	[FormRequestId] INT IDENTITY (1, 1) NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NULL, 
    [Email] NVARCHAR(50) NULL, 
    [Phone] NVARCHAR(50) NULL, 
    [Text] NVARCHAR(MAX) NULL, 
    [CreationDate] DATETIME NULL DEFAULT GETUTCDATE(), 
    [Amount] DECIMAL(18, 2) NULL, 
    [State] VARCHAR(50) NOT NULL DEFAULT 'New', 
    [PaymentState] VARCHAR(50) NULL, 
    [PaymentType] VARCHAR(50) NULL
)
