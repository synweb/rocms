CREATE PROCEDURE [Shop].[Manufacturer_SelectUsed]
AS
	SELECT * FROM [Shop].[Manufacturer]
	WHERE [HeartId] IN 
		(SELECT [ManufacturerId] FROM [Shop].[GoodsItem] WHERE Deleted=0)