CREATE TABLE [Shop].[Action_Goods] (
    [ActionId] INT NOT NULL,
    [GoodsId]   INT NOT NULL,
    CONSTRAINT [PK_ActionGoods] PRIMARY KEY NONCLUSTERED ([ActionId] ASC, [GoodsId] ASC),
    CONSTRAINT [FK_ActionGoods_Action] FOREIGN KEY ([ActionId]) REFERENCES [Shop].[Action] ([ActionId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ActionGoods_Goods] FOREIGN KEY ([GoodsId]) REFERENCES [Shop].[GoodsItem] ([GoodsId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ActionGoods_Goods]
    ON [Shop].[Action_Goods]([GoodsId] ASC);

