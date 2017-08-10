CREATE PROCEDURE [Shop].[Goods_Spec_Delete]
@GoodsId int,
@SpecId int
AS
	DELETE FROM [Shop].[Goods_Spec]
	WHERE [GoodsId]=@GoodsId
		 AND [SpecId]=@SpecId
