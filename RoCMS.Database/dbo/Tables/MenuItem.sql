CREATE TABLE [dbo].[MenuItem] (
    [MenuItemId]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (200) NOT NULL,
    [MenuId]           INT            NOT NULL,
    [ParentMenuItemId] INT            NULL,
    [SortOrder]        INT            DEFAULT ((0)) NOT NULL,
    [BlockId] INT NULL, 
    [HeartId] INT NULL, 
    CONSTRAINT [PK_MenuItem] PRIMARY KEY CLUSTERED ([MenuItemId] ASC),
    CONSTRAINT [FK_MenuItem_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [dbo].[Menu] ([MenuId]) ON DELETE CASCADE ON UPDATE CASCADE,
	CONSTRAINT [FK_MenuItem_HeartId] FOREIGN KEY ([HeartId]) REFERENCES [dbo].[Heart] ([HeartId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MenuItem_ParentMenuItemId] FOREIGN KEY ([ParentMenuItemId]) REFERENCES [dbo].[MenuItem] ([MenuItemId]),
	CONSTRAINT [FK_MenuItem_Block] FOREIGN KEY ([BlockId]) REFERENCES [dbo].[Block] ([BlockId]) ON DELETE CASCADE ON UPDATE CASCADE
);

