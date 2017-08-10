CREATE TABLE [dbo].[Page] (
    [Title]        NVARCHAR (MAX) NOT NULL,
    [Annotation]   NVARCHAR (MAX) NOT NULL,
    [Content]      NVARCHAR (MAX) NOT NULL,
    [CreationDate] DATETIME       NOT NULL DEFAULT GETUTCDATE(),
    [RelativeUrl]  NVARCHAR (300)  NOT NULL,
    [Keywords]     NVARCHAR (MAX) NULL,
    [PageId]       INT            IDENTITY (1, 1) NOT NULL,
    [ParentPageId] INT NULL, 
	[HideInSitemap] BIT NOT NULL DEFAULT 0, 
    [Header] NVARCHAR(MAX) NULL, 
    [Styles] NVARCHAR(MAX) NULL, 
    [Scripts] NVARCHAR(MAX) NULL, 
    [Layout] VARCHAR(300) NOT NULL DEFAULT 'clientLayout', 
	[AdditionalHeaders] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED ([PageId] ASC),
    UNIQUE NONCLUSTERED ([RelativeUrl] ASC), 

    CONSTRAINT [FK_Page_ToPage] FOREIGN KEY ([ParentPageId]) REFERENCES [Page]([PageId])
);

