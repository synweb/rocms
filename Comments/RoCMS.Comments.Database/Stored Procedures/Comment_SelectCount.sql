CREATE PROCEDURE [Comments].[Comment_SelectCount]
	@HeartId int
AS
	SELECT COUNT(*) FROM [Comments].Comment WHERE HeartId = @HeartId
