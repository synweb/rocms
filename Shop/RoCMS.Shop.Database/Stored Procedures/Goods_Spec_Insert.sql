CREATE PROCEDURE [Shop].[Goods_Spec_Insert]
@Value nvarchar(150),
@TranslitedValue NVARCHAR (250),
@HeartId int,
@SpecId int,
@IsPrimary bit
AS
	INSERT INTO [Shop].[Goods_Spec] ([Value], [TranslitedValue], [HeartId], [SpecId], [IsPrimary])
	VALUES (@Value, @TranslitedValue, @HeartId, @SpecId, @IsPrimary)
