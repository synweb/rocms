CREATE PROCEDURE [Shop].[Goods_Pack_Update]
@PackId int,
@GoodsId int,
@Discount int,
@Price decimal
AS
	UPDATE [Shop].[Goods_Pack] SET
		[Price]=@Price,
		[Discount]=@Discount
		WHERE [PackId]=@PackId AND [GoodsId]=@GoodsId
