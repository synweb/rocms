CREATE PROCEDURE [Shop].[CompatibleSet_Goods_Delete]
@HeartId int,
@CompatibleSetId int
AS
	DELETE FROM [Shop].[CompatibleSet_Goods]
	WHERE [HeartId]=@HeartId
		 AND [CompatibleSetId]=@CompatibleSetId
