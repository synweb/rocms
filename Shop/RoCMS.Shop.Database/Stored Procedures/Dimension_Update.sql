CREATE PROCEDURE [Shop].[Dimension_Update]

@Full nvarchar(50),
@Short nvarchar(5),
@DimensionId int
AS
	UPDATE [Shop].[Dimension] SET

		[Full]=@Full,
		[Short]=@Short
	WHERE [DimensionId]=@DimensionId
