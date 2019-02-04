CREATE PROCEDURE [Shop].[Goods_Spec_Update]
@Value nvarchar(150),
@TranslitedValue NVARCHAR (250),
@HeartId int,
@SpecId int,
@IsPrimary bit
AS
	UPDATE [Shop].[Goods_Spec]
		SET [Value]=@Value,
		[TranslitedValue]=@TranslitedValue,
		[IsPrimary]=@IsPrimary
	WHERE [HeartId]=@HeartId AND [SpecId]=@SpecId