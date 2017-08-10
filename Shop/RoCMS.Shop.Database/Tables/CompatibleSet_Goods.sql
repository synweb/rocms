CREATE TABLE [Shop].[CompatibleSet_Goods]
(
	[GoodsId] INT NOT NULL , 
    [CompatibleSetId] INT NOT NULL, 
    PRIMARY KEY ([GoodsId], [CompatibleSetId]),
	CONSTRAINT [FK_CompatibleGoods_GoodsSet] FOREIGN KEY ([GoodsId]) REFERENCES [Shop].[GoodsItem] ([GoodsId]) ON DELETE CASCADE,
    CONSTRAINT [FK_CompatibleGoods_CompatibleSet] FOREIGN KEY ([CompatibleSetId]) REFERENCES [Shop].[CompatibleSet] ([CompatibleSetId]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_CompatibleGoods_Column] ON [Shop].[CompatibleSet_Goods] ([GoodsId])
