CREATE TABLE [Shop].[Goods_Pack]
(
	[PackId] INT NOT NULL , 
    [GoodsId] INT NOT NULL, 
    [Discount] INT NULL, 
    [Price] DECIMAL(18, 2) NULL, 
    PRIMARY KEY ([GoodsId], [PackId]),
	CONSTRAINT [FK_GoodsPack_Pack] FOREIGN KEY ([PackId]) REFERENCES [Shop].[Pack] ([PackId]) ON DELETE CASCADE,
    CONSTRAINT [FK_GoodsPack_Goods] FOREIGN KEY ([GoodsId]) REFERENCES [Shop].[GoodsItem] ([GoodsId]) ON DELETE CASCADE
)
