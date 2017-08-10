CREATE TABLE [dbo].[CmsResource] (
    [CmsResourceId] INT          IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50) NOT NULL UNIQUE,
    [Description]	NVARCHAR (200) NOT NULL DEFAULT '',
    PRIMARY KEY CLUSTERED ([CmsResourceId] ASC)
);

