CREATE PROCEDURE [Shop].[CompatibleSet_Goods_Delete]
@GoodsId int,
@CompatibleSetId int
AS
	DELETE FROM [Shop].[CompatibleSet_Goods]
	WHERE [GoodsId]=@GoodsId
		 AND [CompatibleSetId]=@CompatibleSetId
