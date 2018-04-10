CREATE TABLE [Shop].[Manufacturer] (
    [HeartId] INT NOT NULL,
    [Guid]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() UNIQUE,
    [Name]           NVARCHAR (250) NOT NULL,
    [LogoImageId]    VARCHAR (30)   NULL,
    [Description]    NVARCHAR (MAX) NULL,
	[CountryId]		INT NULL,
    CONSTRAINT [PK_ManufacturerSet] PRIMARY KEY CLUSTERED ([HeartId] ASC),
    CONSTRAINT [FK_ManufacturerSet_Image] FOREIGN KEY ([LogoImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL,
    CONSTRAINT [FK_ManufacturerSet_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([CountryId]) ON DELETE SET NULL
);

