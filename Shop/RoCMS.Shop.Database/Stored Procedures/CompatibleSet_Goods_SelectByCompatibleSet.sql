CREATE PROCEDURE [Shop].[CompatibleSet_Goods_SelectByCompatibleSet]
@CompatibleSetId int
AS
	SELECT * FROM [Shop].[CompatibleSet_Goods]
		WHERE [CompatibleSetId]=@CompatibleSetId
