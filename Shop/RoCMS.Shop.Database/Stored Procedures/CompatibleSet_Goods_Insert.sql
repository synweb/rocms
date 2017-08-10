CREATE PROCEDURE [Shop].[CompatibleSet_Goods_Insert]
@GoodsId int,
@CompatibleSetId int
AS
	INSERT INTO [Shop].[CompatibleSet_Goods] ([GoodsId], [CompatibleSetId])
	VALUES (@GoodsId, @CompatibleSetId)
