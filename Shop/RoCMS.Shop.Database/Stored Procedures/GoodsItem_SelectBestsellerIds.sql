﻿CREATE PROCEDURE [Shop].[GoodsItem_SelectBestsellerIds]
	@Count int = 10
AS

	SELECT HeartId FROM (
	select top (@Count) g.HeartId, Count(g.HeartId) as SellsCount from Shop.GoodsItem g JOIN Shop.GoodsInOrder gio on g.HeartId=gio.HeartId
	WHERE g.Deleted=0 AND g.NotAvailable=0 AND gio.OrderId IN (SELECT TOP(100) OrderId FROM Shop.[Order] ORDER BY OrderId DESC)
	GROUP BY g.HeartId
	ORDER BY SellsCount DESC) t