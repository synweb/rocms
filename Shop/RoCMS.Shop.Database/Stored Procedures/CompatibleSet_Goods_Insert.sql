CREATE PROCEDURE [Shop].[CompatibleSet_Goods_Insert]
@HeartId int,
@CompatibleSetId int
AS
	INSERT INTO [Shop].[CompatibleSet_Goods] ([HeartId], [CompatibleSetId])
	VALUES (@HeartId, @CompatibleSetId)
