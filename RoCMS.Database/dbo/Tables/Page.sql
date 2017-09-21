CREATE TABLE [dbo].[Page] (
	[HeartId]	int	NOT NULL,
    [Content]      NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED ([HeartId] ASC),
    CONSTRAINT [FK_Page_Heart] FOREIGN KEY ([HeartId]) REFERENCES [Heart]([HeartId]) ON DELETE CASCADE
);

