CREATE PROCEDURE [Shop].[Goods_GetCountries]
@HeartIds [Int_Table] readonly
AS
	SELECT DISTINCT c.CountryId AS ID, c.Name FROM 
	GoodsItem g JOIN Manufacturer m ON g.ManufacturerId=m.ManufacturerId
		JOIN Country c ON m.CountryId=c.CountryId
	 WHERE HeartId IN (SELECT * FROM @HeartIds)