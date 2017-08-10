CREATE TABLE [dbo].[Slide] (
    [SlideId]     INT            IDENTITY (1, 1) NOT NULL,
    [SliderId]    INT            NOT NULL,
    [Title]       NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    [ImageId]     VARCHAR(30)   NOT NULL,
    [Link]        NVARCHAR (MAX) NULL,
	[SortOrder] INT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Slide] PRIMARY KEY NONCLUSTERED ([SlideId] ASC),
    CONSTRAINT [FK_Slide_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE CASCADE,
    CONSTRAINT [FK_Slide_SliderId] FOREIGN KEY ([SliderId]) REFERENCES [dbo].[Slider] ([SliderId]) ON DELETE CASCADE
);
GO
CREATE CLUSTERED INDEX [IX_Slide_Sort] ON [dbo].[Slide] ([SortOrder] ASC)
