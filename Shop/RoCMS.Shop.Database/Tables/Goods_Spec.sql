CREATE TABLE [Shop].[Goods_Spec] (
    [Value]   NVARCHAR (150) NOT NULL,
    [GoodsId] INT           NOT NULL,
    [SpecId]  INT           NOT NULL,
    [IsPrimary] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_GoodsSpecs] PRIMARY KEY CLUSTERED ([GoodsId] ASC, [SpecId] ASC),
    CONSTRAINT [FK_GoodsGoodsSpec] FOREIGN KEY ([GoodsId]) REFERENCES [Shop].[GoodsItem] ([GoodsId]) ON DELETE CASCADE,
    CONSTRAINT [FK_SpecGoodsSpec] FOREIGN KEY ([SpecId]) REFERENCES [Shop].[Spec] ([SpecId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_SpecGoodsSpec]
    ON [Shop].[Goods_Spec]([SpecId] ASC);

