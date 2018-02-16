CREATE PROCEDURE [Shop].[Goods_Pack_Update]
@PackId int,
@HeartId int,
@Discount int,
@Price decimal
AS
	UPDATE [Shop].[Goods_Pack] SET
		[Price]=@Price,
		[Discount]=@Discount
		WHERE [PackId]=@PackId AND [HeartId]=@HeartId
