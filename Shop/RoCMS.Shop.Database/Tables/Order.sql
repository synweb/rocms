CREATE TABLE [Shop].[Order] (
    [OrderId]      INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate]  DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Address]      NVARCHAR (MAX) NULL,
    [ClientId]     INT            NOT NULL,
    [Comment]      NVARCHAR (MAX) NULL,
    [AdminComment]      NVARCHAR (MAX) NULL,
    [State]        NVARCHAR (20)  NOT NULL,
    [ShipmentDate] DATETIME       NULL,
    [ShipmentType] NVARCHAR (20)  NOT NULL,
    [PickUpPointId] INT NULL, 
    [DeliveryPrice] DECIMAL(18, 2) NULL, 
    [TotalDiscount] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_OrderSet] PRIMARY KEY CLUSTERED ([OrderId] ASC),
    CONSTRAINT [FK_OrderClient] FOREIGN KEY ([ClientId]) REFERENCES [Shop].[Client] ([ClientId]) ON DELETE CASCADE,
	CONSTRAINT [FK_OrderPickUpPoint] FOREIGN KEY ([PickUpPointId]) REFERENCES [Shop].[PickUpPoint] ([PickUpPointId]) ON Delete SET NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_OrderClient]
    ON [Shop].[Order]([ClientId] ASC);

