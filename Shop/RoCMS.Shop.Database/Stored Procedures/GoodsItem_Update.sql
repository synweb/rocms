CREATE PROCEDURE [Shop].[GoodsItem_Update]
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

@Filename nvarchar(200),
@HeartId int
AS
	UPDATE [Shop].[GoodsItem] SET
		[Name]=@Name,
		[ManufacturerId]=@ManufacturerId,
		[SupplierId]=@SupplierId,
		[Price]=@Price,
		[Currency]=@Currency,
		
		[Description]=@Description,
		[HtmlDescription]=@HtmlDescription,
		[MainImageId]=@MainImageId,
		[Article]=@Article,
		
		[NotAvailable]=@NotAvailable,
		[BasePackId]=@BasePackId,
		
		[Filename]=@Filename
	WHERE [HeartId]=@HeartId
