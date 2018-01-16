CREATE TABLE [Shop].[GoodsInOrder] (
    [Quantity] INT NOT NULL,
    [HeartId]  INT            NOT NULL,
    [OrderId]  INT            NOT NULL,
    [PackId] INT NULL, 
    [Id] INT NOT NULL IDENTITY, 
    [Price] DECIMAL(18, 2) NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_GoodsInOrderSet] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_GoodsInOrderGoods] FOREIGN KEY ([HeartId]) REFERENCES [Shop].[GoodsItem] ([HeartId]),
    CONSTRAINT [FK_GoodsInOrderOrder] FOREIGN KEY ([OrderId]) REFERENCES [Shop].[Order] ([OrderId]) ON DELETE CASCADE,
	CONSTRAINT [FK_GoodsInOrderPack] FOREIGN KEY ([PackId]) REFERENCES [Shop].[Pack] ([PackId]) ON DELETE SET NULL, 
    CONSTRAINT [AK_GoodsInOrderSet_Column] UNIQUE ([HeartId], [OrderId], [PackId])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_GoodsInOrderOrder]
    ON [Shop].[GoodsInOrder]([OrderId] ASC);


GO
