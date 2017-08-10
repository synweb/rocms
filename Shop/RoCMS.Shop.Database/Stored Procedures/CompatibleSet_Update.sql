CREATE PROCEDURE [Shop].[CompatibleSet_Update]
@Guid uniqueidentifier,
@Name nvarchar(50),
@CompatibleSetId int
AS
	UPDATE [Shop].[CompatibleSet] SET
		[Guid]=@Guid,
		[Name]=@Name
	WHERE [CompatibleSetId]=@CompatibleSetId
