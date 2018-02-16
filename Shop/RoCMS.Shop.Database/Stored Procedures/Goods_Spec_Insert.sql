CREATE PROCEDURE [Shop].[Goods_Spec_Insert]
@Value nvarchar(150),
@HeartId int,
@SpecId int,
@IsPrimary bit
AS
	INSERT INTO [Shop].[Goods_Spec] ([Value], [HeartId], [SpecId], [IsPrimary])
	VALUES (@Value, @HeartId, @SpecId, @IsPrimary)
