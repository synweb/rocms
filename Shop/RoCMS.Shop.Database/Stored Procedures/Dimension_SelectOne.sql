CREATE PROCEDURE [Shop].[Dimension_SelectOne]
@DimensionId int
AS
	SELECT * FROM [Shop].[Dimension]
	WHERE [DimensionId]=@DimensionId