CREATE TABLE [dbo].[Block] (
    [BlockId] INT            IDENTITY (1, 1) NOT NULL,
    [Title]   NVARCHAR (MAX) NOT NULL,
    [Content] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED ([BlockId] ASC)
);

