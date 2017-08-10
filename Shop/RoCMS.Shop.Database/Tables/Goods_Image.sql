CREATE TABLE [Shop].[Goods_Image] (
    [GoodsId] INT          NOT NULL,
    [ImageId]                 VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_GoodsImage] PRIMARY KEY NONCLUSTERED ([GoodsId] ASC, [ImageId] ASC),
    CONSTRAINT [FK_GoodsImage_Goods] FOREIGN KEY ([GoodsId]) REFERENCES [Shop].[GoodsItem] ([GoodsId]) ON DELETE CASCADE,
    CONSTRAINT [FK_GoodsImage_Image] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_GoodsImage_Image]
    ON [Shop].[Goods_Image]([ImageId] ASC);

