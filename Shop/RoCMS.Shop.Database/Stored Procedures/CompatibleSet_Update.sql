CREATE PROCEDURE [Shop].[CompatibleSet_Update]

@Name nvarchar(50),
@CompatibleSetId int
AS
	UPDATE [Shop].[CompatibleSet] SET

		[Name]=@Name
	WHERE [CompatibleSetId]=@CompatibleSetId
