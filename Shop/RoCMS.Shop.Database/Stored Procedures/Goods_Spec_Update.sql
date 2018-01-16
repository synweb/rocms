CREATE PROCEDURE [Shop].[Goods_Spec_Update]
@Value nvarchar(150),
@HeartId int,
@SpecId int,
@IsPrimary bit
AS
	UPDATE [Shop].[Goods_Spec]
		SET [Value]=@Value,
		[IsPrimary]=@IsPrimary
	WHERE [HeartId]=@HeartId AND [SpecId]=@SpecId