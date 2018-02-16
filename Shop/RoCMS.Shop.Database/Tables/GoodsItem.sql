CREATE TABLE [Shop].[GoodsItem] (
    [HeartId]         INT NOT NULL,
    [Guid]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() UNIQUE,
    [Name]            NVARCHAR (500) NOT NULL,
    [ManufacturerId]  INT            NULL,
	[SupplierId]      INT            NULL,
    [Price]           DECIMAL (18, 2)   NULL,
	[Currency]           VARCHAR(3) NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [HtmlDescription] NVARCHAR (MAX) NULL,
    [MainImageId] VARCHAR(30) NULL, 
    [Article] NVARCHAR(50) NULL, 
    [NotAvailable] BIT NOT NULL DEFAULT 0, 
    [BasePackId] INT NULL, 
    [Deleted] BIT NOT NULL DEFAULT 0, 
	[Filename] NVARCHAR(200) NULL,
    CONSTRAINT [PK_Goods] PRIMARY KEY CLUSTERED ([HeartId] ASC),
	CONSTRAINT [FK_Goods_Heart] FOREIGN KEY ([HeartId]) REFERENCES [Heart]([HeartId]) ON DELETE CASCADE,

    CONSTRAINT [FK_GoodsManufacturer] FOREIGN KEY ([ManufacturerId]) REFERENCES [Shop].[Manufacturer] ([ManufacturerId]),
	CONSTRAINT [FK_Goods_Supplier_Manufacturer] FOREIGN KEY ([SupplierId]) REFERENCES [Shop].[Manufacturer] ([ManufacturerId]),
	CONSTRAINT [FK_GoodsImage] FOREIGN KEY ([MainImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL,
	CONSTRAINT [FK_Goods_Pack] FOREIGN KEY ([BasePackId]) REFERENCES [Shop].[Pack] ([PackId]) ON DELETE SET NULL, 
    CONSTRAINT [FK_Goods_Currency] FOREIGN KEY ([Currency]) REFERENCES [Shop].[Currency]([CurrencyId])
);
GO

CREATE UNIQUE INDEX [IX_GoodsSet_Article_ManufacturerId] ON [Shop].[GoodsItem] ([Article], [ManufacturerId]) WHERE ([Article] IS NOT NULL AND [Deleted] = 0)
GO

CREATE NONCLUSTERED INDEX [IX_FK_GoodsManufacturer]
    ON [Shop].[GoodsItem]([ManufacturerId] ASC);

	GO

CREATE INDEX [IX_GoodsSet_Name] ON [Shop].[GoodsItem] ([Name])
GO

CREATE INDEX [IX_GoodsSet_Article] ON [Shop].[GoodsItem] ([Article])
GO

CREATE INDEX [IX_GoodsSet_SupplierId] ON [Shop].[GoodsItem] ([SupplierId])
GO

