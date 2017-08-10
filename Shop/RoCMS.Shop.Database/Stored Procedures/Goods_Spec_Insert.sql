CREATE PROCEDURE [Shop].[Goods_Spec_Insert]
@Value nvarchar(150),
@GoodsId int,
@SpecId int,
@IsPrimary bit
AS
	INSERT INTO [Shop].[Goods_Spec] ([Value], [GoodsId], [SpecId], [IsPrimary])
	VALUES (@Value, @GoodsId, @SpecId, @IsPrimary)
