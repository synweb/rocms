CREATE TABLE [Shop].[Category] (
    [HeartId]       INT NOT NULL,
    [Guid]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() UNIQUE,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [ParentCategoryId] INT            NULL,
    [Description] NVARCHAR(MAX) NULL, 
    [SortOrder] INT NOT NULL DEFAULT 0, 
    [ImageId] VARCHAR(30) NULL, 
    [Hidden] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_CategorySet] PRIMARY KEY CLUSTERED ([HeartId] ASC),
    CONSTRAINT [FK_Category_Image] FOREIGN KEY ([ImageId]) REFERENCES [Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL,
	CONSTRAINT [FK_Category_Heart] FOREIGN KEY ([HeartId]) REFERENCES [Heart]([HeartId])-- ON DELETE CASCADE

);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ParentCategoryId]
    ON [Shop].[Category]([ParentCategoryId] ASC);
	GO
