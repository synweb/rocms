CREATE TABLE [Shop].[Spec_Category]
(
	[CategoryId] INT NOT NULL,
	[SpecId] INT NOT NULL, 
    CONSTRAINT [PK_SpecCategory] PRIMARY KEY ([CategoryId], [SpecId]), 
    CONSTRAINT [FK_SpecCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Shop].[Category]([CategoryId]) ON DELETE CASCADE, 
    CONSTRAINT [FK_SpecCategory_Spec] FOREIGN KEY ([SpecId]) REFERENCES [Shop].[Spec]([SpecId]) ON DELETE CASCADE,
)
