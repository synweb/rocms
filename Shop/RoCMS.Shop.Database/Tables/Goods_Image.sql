CREATE TABLE [Shop].[Goods_Image] (
    [HeartId] INT          NOT NULL,
    [ImageId]                 VARCHAR (30) NOT NULL,
    [SortOrder] INT NOT NULL DEFAULT 1, 
    [CreationDate]  DATETIME NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_GoodsImage] PRIMARY KEY NONCLUSTERED ([HeartId] ASC, [ImageId] ASC),
    CONSTRAINT [FK_GoodsImage_Goods] FOREIGN KEY ([HeartId]) REFERENCES [Shop].[GoodsItem] ([HeartId]) ON DELETE CASCADE,
    CONSTRAINT [FK_GoodsImage_Image] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_GoodsImage_Image]
    ON [Shop].[Goods_Image]([ImageId] ASC);

