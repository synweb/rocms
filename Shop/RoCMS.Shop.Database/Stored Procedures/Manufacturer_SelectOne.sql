CREATE PROCEDURE [Shop].[Manufacturer_SelectOne]
@HeartId int
AS
	SELECT * FROM [Shop].[Manufacturer]
	WHERE [HeartId]=@HeartId
