CREATE TABLE [dbo].[Heart]
(
	[HeartId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [RelativeUrl]  NVARCHAR (300) NOT NULL,
    [ParentHeartId] INT NULL, 
	[BreadcrumbsTitle] NVARCHAR (MAX),
	[Noindex] BIT NOT NULL DEFAULT 0,
    [Title] NVARCHAR (MAX) NOT NULL,
    [MetaDescription] NVARCHAR (MAX) NOT NULL,
    [MetaKeywords] NVARCHAR (MAX) NOT NULL,
    [Styles] NVARCHAR(MAX) NULL, 
    [Scripts] NVARCHAR(MAX) NULL, 
    [Layout] VARCHAR(300) NOT NULL DEFAULT 'clientLayout', 
	[AdditionalHeaders] NVARCHAR (MAX) NULL,
    UNIQUE NONCLUSTERED ([RelativeUrl] ASC), 
    CONSTRAINT [FK_Heart_ChildHeart] FOREIGN KEY ([ParentHeartId]) REFERENCES [Heart]([HeartId])
	
)
