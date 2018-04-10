CREATE PROCEDURE [Shop].[Manufacturer_SelectSuppliers]
AS
	SELECT * FROM [Shop].[Manufacturer]
	WHERE [HeartId] IN 
		(SELECT SupplierId FROM [Shop].[GoodsItem] WHERE Deleted=0)