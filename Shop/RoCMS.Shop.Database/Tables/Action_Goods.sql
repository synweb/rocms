CREATE TABLE [Shop].[Action_Goods] (
    [ActionId] INT NOT NULL,
    [HeartId]   INT NOT NULL,
    CONSTRAINT [PK_ActionGoods] PRIMARY KEY NONCLUSTERED ([ActionId] ASC, [HeartId] ASC),
    CONSTRAINT [FK_ActionGoods_Action] FOREIGN KEY ([ActionId]) REFERENCES [Shop].[Action] ([HeartId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ActionGoods_Goods] FOREIGN KEY ([HeartId]) REFERENCES [Shop].[GoodsItem] ([HeartId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_ActionGoods_Goods]
    ON [Shop].[Action_Goods]([HeartId] ASC);

