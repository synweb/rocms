CREATE PROCEDURE [Shop].[GoodsItem_Insert]
@Name nvarchar(500),
@ManufacturerId int,
@SupplierId int,
@Price decimal,
@Currency varchar(3),
@Keywords nvarchar(MAX),
@Description nvarchar(MAX),
@HtmlDescription nvarchar(MAX),
@MainImageId varchar(30),
@Article nvarchar(50),
@SearchDescription nvarchar(MAX),
@NotAvailable bit,
@BasePackId int,
@Deleted bit,
@RelativeUrl nvarchar(300),
@Filename nvarchar(200)
AS
	INSERT INTO [Shop].[GoodsItem] ([Name], [ManufacturerId], [SupplierId], [Price], [Currency], [Keywords], [Description], [HtmlDescription], [MainImageId], [Article], [SearchDescription], [NotAvailable], [BasePackId], [Deleted], [RelativeUrl], [Filename])
	VALUES (@Name, @ManufacturerId, @SupplierId, @Price, @Currency, @Keywords, @Description, @HtmlDescription, @MainImageId, @Article, @SearchDescription, @NotAvailable, @BasePackId, @Deleted, @RelativeUrl, @Filename)
	SELECT @@IDENTITY
