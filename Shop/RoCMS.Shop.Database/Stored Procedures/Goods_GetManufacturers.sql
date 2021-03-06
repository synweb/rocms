﻿CREATE PROCEDURE [Shop].[Goods_GetManufacturers]
	@HeartIds [Int_Table] readonly
AS
	SELECT DISTINCT g.ManufacturerId AS ID, m.Name FROM [Shop].GoodsItem g JOIN [Shop].Manufacturer m ON g.ManufacturerId=m.HeartId WHERE g.HeartId IN (SELECT * FROM @HeartIds)