CREATE PROCEDURE [Shop].[Manufacturer_Delete]
@ManufacturerId int
AS
	DELETE FROM [Shop].[Manufacturer]
	WHERE [ManufacturerId]=@ManufacturerId
