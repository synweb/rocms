CREATE TABLE [Shop].[Category] (
    [CategoryId]       INT            IDENTITY (1, 1) NOT NULL,
	[CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Guid]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() UNIQUE,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [ParentCategoryId] INT            NULL,
    [Description] NVARCHAR(MAX) NULL, 
    [SortOrder] INT NOT NULL DEFAULT 0, 
    [MetaDescription] NVARCHAR(MAX) NULL, 
    [ImageId] VARCHAR(30) NULL, 
    [Hidden] BIT NOT NULL DEFAULT 0, 
    [RelativeUrl] NVARCHAR(300) NOT NULL, 
    CONSTRAINT [PK_CategorySet] PRIMARY KEY CLUSTERED ([CategoryId] ASC),
    CONSTRAINT [FK_Category_Image] FOREIGN KEY ([ImageId]) REFERENCES [Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL


);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ParentCategoryId]
    ON [Shop].[Category]([ParentCategoryId] ASC);
	GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FK_CategoriesRelativeUrl]
    ON [Shop].[Category]([RelativeUrl] ASC);
GO