CREATE TABLE [Shop].[CompatibleSet_Goods]
(
	[HeartId] INT NOT NULL , 
    [CompatibleSetId] INT NOT NULL, 
    PRIMARY KEY ([HeartId], [CompatibleSetId]),
	CONSTRAINT [FK_CompatibleGoods_GoodsSet] FOREIGN KEY ([HeartId]) REFERENCES [Shop].[GoodsItem] ([HeartId]) ON DELETE CASCADE,
    CONSTRAINT [FK_CompatibleGoods_CompatibleSet] FOREIGN KEY ([CompatibleSetId]) REFERENCES [Shop].[CompatibleSet] ([CompatibleSetId]) ON DELETE CASCADE
)

GO

CREATE INDEX [IX_CompatibleGoods_Column] ON [Shop].[CompatibleSet_Goods] ([HeartId])
