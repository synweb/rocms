CREATE TABLE [News].[Category]
(
    [CategoryId]       INT IDENTITY (1, 1) NOT NULL PRIMARY KEY,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Name]             NVARCHAR (MAX) NOT NULL,
    [ParentCategoryId] INT            NULL,
    [SortOrder] INT NOT NULL DEFAULT 0, 
    [Hidden] BIT NOT NULL DEFAULT 0, 
    [RelativeUrl] NVARCHAR(300) NOT NULL , 
    CONSTRAINT [FK_Category_Parent] FOREIGN KEY ([ParentCategoryId]) REFERENCES [News].[Category]([CategoryId]),
	UNIQUE NONCLUSTERED ([RelativeUrl] ASC)
	 
)
