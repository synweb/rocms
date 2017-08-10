CREATE PROCEDURE [Shop].[CompatibleSet_SelectOne]
@CompatibleSetId int
AS
	SELECT * FROM [Shop].[CompatibleSet]
	WHERE [CompatibleSetId]=@CompatibleSetId