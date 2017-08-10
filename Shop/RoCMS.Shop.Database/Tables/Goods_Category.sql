CREATE TABLE [Shop].[Goods_Category] (
    [GoodsId]       INT NOT NULL,
    [CategoryId] INT NOT NULL,
    CONSTRAINT [PK_GoodsCategory] PRIMARY KEY NONCLUSTERED ([GoodsId] ASC, [CategoryId] ASC),
    CONSTRAINT [FK_GoodsCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Shop].[Category] ([CategoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_GoodsCategory_Goods] FOREIGN KEY ([GoodsId]) REFERENCES [Shop].[GoodsItem] ([GoodsId]) ON DELETE CASCADE
);


GO
CREATE CLUSTERED INDEX [IX_FK_GoodsCategory_Category]
    ON [Shop].[Goods_Category]([CategoryId] ASC);
GO
CREATE NONCLUSTERED INDEX [IX_FK_GoodsCategory_Goods]
    ON [Shop].[Goods_Category]([GoodsId] ASC);
