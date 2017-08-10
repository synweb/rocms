CREATE PROCEDURE [Shop].[Manufacturer_SelectManufacturerCountries]
AS
	SELECT * FROM [dbo].[Country]
	WHERE [CountryId] IN 
		(SELECT [CountryId] FROM [Shop].[Manufacturer])
