CREATE TABLE [Shop].[Goods_Pack]
(
	[PackId] INT NOT NULL , 
    [HeartId] INT NOT NULL, 
    [Discount] INT NULL, 
    [Price] DECIMAL(18, 2) NULL, 
    PRIMARY KEY ([HeartId], [PackId]),
	CONSTRAINT [FK_GoodsPack_Pack] FOREIGN KEY ([PackId]) REFERENCES [Shop].[Pack] ([PackId]) ON DELETE CASCADE,
    CONSTRAINT [FK_GoodsPack_Goods] FOREIGN KEY ([HeartId]) REFERENCES [Shop].[GoodsItem] ([HeartId]) ON DELETE CASCADE
)
