CREATE PROCEDURE [Shop].[Goods_GetCountries]
@GoodsIds [Int_Table] readonly
AS
	SELECT DISTINCT c.CountryId AS ID, c.Name FROM 
	GoodsItem g JOIN Manufacturer m ON g.ManufacturerId=m.ManufacturerId
		JOIN Country c ON m.CountryId=c.CountryId
	 WHERE GoodsId IN (SELECT * FROM @GoodsIds)