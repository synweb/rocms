CREATE TABLE [dbo].[Slider] (
    [SliderId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Slider] PRIMARY KEY CLUSTERED ([SliderId] ASC)
);

