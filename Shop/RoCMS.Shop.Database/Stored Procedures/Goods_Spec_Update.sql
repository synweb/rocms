CREATE PROCEDURE [Shop].[Goods_Spec_Update]
@Value nvarchar(150),
@GoodsId int,
@SpecId int,
@IsPrimary bit
AS
	UPDATE [Shop].[Goods_Spec]
		SET [Value]=@Value,
		[IsPrimary]=@IsPrimary
	WHERE [GoodsId]=@GoodsId AND [SpecId]=@SpecId