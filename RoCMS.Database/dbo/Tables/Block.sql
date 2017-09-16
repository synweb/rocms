CREATE TABLE [dbo].[Block] (
    [BlockId] INT            IDENTITY (1, 1) NOT NULL,
	[Name] NVARCHAR(200) NULL,
    [Title]   NVARCHAR (MAX) NOT NULL,
    [Content] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED ([BlockId] ASC)
);
GO
CREATE UNIQUE INDEX [IX_Block_Name] ON [dbo].[Block]  ([Name]) WHERE ([Name] IS NOT NULL)
GO