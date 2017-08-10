CREATE TABLE [Shop].[Pack]
(
	[PackId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CreationDate]  DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Guid]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() UNIQUE,
    [Name] NVARCHAR(50) NOT NULL, 
    [Size] FLOAT NOT NULL, 
    [DimensionId] INT NOT NULL,
	[DefaultDiscount] INT NULL, 
    CONSTRAINT [FK_Pack_Dimension] FOREIGN KEY ([DimensionId]) REFERENCES [Shop].[Dimension] ([DimensionId])
)
