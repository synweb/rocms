CREATE PROCEDURE [Shop].[Dimension_Delete]
@DimensionId int
AS
	DELETE FROM [Shop].[Dimension]
	WHERE [DimensionId]=@DimensionId
