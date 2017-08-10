CREATE PROCEDURE [Shop].[Dimension_Update]
@Guid uniqueidentifier,
@Full nvarchar(50),
@Short nvarchar(5),
@DimensionId int
AS
	UPDATE [Shop].[Dimension] SET
		[Guid]=@Guid,
		[Full]=@Full,
		[Short]=@Short
	WHERE [DimensionId]=@DimensionId
