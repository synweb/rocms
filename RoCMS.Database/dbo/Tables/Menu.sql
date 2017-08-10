CREATE TABLE [dbo].[Menu] (
    [MenuId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([MenuId] ASC)
);

