CREATE PROCEDURE [Shop].[Manufacturer_SelectOne]
@ManufacturerId int
AS
	SELECT * FROM [Shop].[Manufacturer]
	WHERE [ManufacturerId]=@ManufacturerId
