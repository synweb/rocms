CREATE PROCEDURE [Shop].[Action_Manufacturer_SelectByManufacturer]
@ManufacturerId int
AS
	SELECT * FROM [Shop].[Action_Manufacturer]
		WHERE [ManufacturerId]=@ManufacturerId
