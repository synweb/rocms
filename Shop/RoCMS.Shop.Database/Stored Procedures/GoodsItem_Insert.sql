CREATE PROCEDURE [Shop].[GoodsItem_Insert]
@HeartId int,
@Name nvarchar(500),
@ManufacturerId int,
@SupplierId int,

@Price decimal,
@Currency varchar(3),

@Description nvarchar(MAX),
@HtmlDescription nvarchar(MAX),
@MainImageId varchar(30),
@Article nvarchar(50),

@NotAvailable bit,
@BasePackId int,
@Deleted bit,

@Filename nvarchar(200)
AS
	INSERT INTO [Shop].[GoodsItem] ([HeartId], [Name], [ManufacturerId], [SupplierId], [Price], [Currency], [Description], [HtmlDescription], [MainImageId], [Article], [NotAvailable], [BasePackId], [Deleted], [Filename])
	VALUES (@HeartId, @Name, @ManufacturerId, @SupplierId, @Price, @Currency, @Description, @HtmlDescription, @MainImageId, @Article, @NotAvailable, @BasePackId, @Deleted, @Filename)
	SELECT @@IDENTITY
