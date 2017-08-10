﻿CREATE PROCEDURE [Shop].[GoodsItem_Update]
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
@RelativeUrl nvarchar(300),
@Filename nvarchar(200),
@GoodsId int
AS
	UPDATE [Shop].[GoodsItem] SET
		[Name]=@Name,
		[ManufacturerId]=@ManufacturerId,
		[SupplierId]=@SupplierId,
		[Price]=@Price,
		[Currency]=@Currency,
		[Keywords]=@Keywords,
		[Description]=@Description,
		[HtmlDescription]=@HtmlDescription,
		[MainImageId]=@MainImageId,
		[Article]=@Article,
		[SearchDescription]=@SearchDescription,
		[NotAvailable]=@NotAvailable,
		[BasePackId]=@BasePackId,
		[RelativeUrl]=@RelativeUrl,
		[Filename]=@Filename
	WHERE [GoodsId]=@GoodsId
