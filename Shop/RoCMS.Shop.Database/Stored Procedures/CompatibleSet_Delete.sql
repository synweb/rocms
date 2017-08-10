CREATE PROCEDURE [Shop].[CompatibleSet_Delete]
@CompatibleSetId int
AS
	DELETE FROM [Shop].[CompatibleSet]
	WHERE [CompatibleSetId]=@CompatibleSetId
