CREATE VIEW [Shop].[GoodsWithActualDiscounts]
	AS 
SELECT        Shop.GoodsItem.HeartId, Shop.GoodsItem.Name, Shop.GoodsItem.ManufacturerId, Shop.GoodsItem.SupplierId, Shop.GoodsItem.NotAvailable, Shop.GoodsItem.Price, 
                         Shop.GoodsItem.Description, Shop.GoodsItem.HtmlDescription, Shop.GoodsItem.MainImageId, Shop.GoodsItem.Article, Shop.GoodsItem.Currency,  MAX(t.Discount) AS Discount, Shop.GoodsItem.Price * (100 - COALESCE (t.Discount, 0)) 
                         / 100 AS ActualPrice
FROM            Shop.GoodsItem LEFT OUTER JOIN
                             (SELECT        GoodsItem_3.HeartId ggg, GoodsItem_3.Name, GoodsItem_3.ManufacturerId, GoodsItem_3.Price, GoodsItem_3.Description, 
                                                         GoodsItem_3.HtmlDescription, GoodsItem_3.MainImageId, GoodsItem_3.Article, GoodsItem_3.Currency, Shop.Action.Discount, Shop.Action.HeartId, Shop.Action.DateOfEnding
                               FROM            Shop.GoodsItem AS GoodsItem_3 INNER JOIN
                                                         Shop.Action_Goods INNER JOIN
                                                         Shop.Action ON Shop.Action_Goods.ActionId = Shop.Action.HeartId ON GoodsItem_3.HeartId = Shop.Action_Goods.HeartId
                               WHERE        (Shop.Action.DateOfEnding > GETUTCDATE() OR
                                                         Shop.Action.DateOfEnding IS NULL) AND (Shop.Action.Active = 1)
                               UNION
                               SELECT        GoodsItem_2.HeartId ggg, GoodsItem_2.Name, GoodsItem_2.ManufacturerId, GoodsItem_2.Price, GoodsItem_2.Description, 
                                                        GoodsItem_2.HtmlDescription, GoodsItem_2.MainImageId, GoodsItem_2.Article, GoodsItem_2.Currency, Action_2.Discount, Action_2.HeartId, Action_2.DateOfEnding
                               FROM            Shop.Action_Manufacturer INNER JOIN
                                                        Shop.Action AS Action_2 ON Shop.Action_Manufacturer.ActionId = Action_2.HeartId INNER JOIN
                                                        Shop.Manufacturer ON Shop.Action_Manufacturer.ManufacturerId = Shop.Manufacturer.HeartId INNER JOIN
                                                        Shop.GoodsItem AS GoodsItem_2 ON Shop.Manufacturer.HeartId = GoodsItem_2.ManufacturerId
                               WHERE        (Action_2.DateOfEnding > GETUTCDATE() OR
                                                        Action_2.DateOfEnding IS NULL) AND (Action_2.Active = 1)
                               UNION
                               SELECT        GoodsItem_1.HeartId AS ggg, GoodsItem_1.Name, GoodsItem_1.ManufacturerId, GoodsItem_1.Price, GoodsItem_1.Description, 
                                                        GoodsItem_1.HtmlDescription, GoodsItem_1.MainImageId, GoodsItem_1.Article, GoodsItem_1.Currency, Action_1.Discount, Action_1.HeartId, Action_1.DateOfEnding
                               FROM            Shop.Action AS Action_1 INNER JOIN
                                                        Shop.Action_Category ON Action_1.HeartId = Shop.Action_Category.ActionId INNER JOIN
                                                        Shop.Category ON Shop.Action_Category.CategoryId = Shop.Category.HeartId INNER JOIN
                                                        Shop.Goods_Category ON Shop.Category.HeartId = Shop.Goods_Category.CategoryId INNER JOIN
                                                        Shop.GoodsItem AS GoodsItem_1 ON Shop.Goods_Category.GoodsId = GoodsItem_1.HeartId
                               WHERE        (Action_1.DateOfEnding > GETUTCDATE() OR
                                                        Action_1.DateOfEnding IS NULL) AND (Action_1.Active = 1)) AS t ON t.ggg = Shop.GoodsItem.HeartId
GROUP BY Shop.GoodsItem.HeartId, Shop.GoodsItem.Name, Shop.GoodsItem.ManufacturerId, Shop.GoodsItem.SupplierId, Shop.GoodsItem.NotAvailable, Shop.GoodsItem.Price, 
                         Shop.GoodsItem.Description, Shop.GoodsItem.HtmlDescription, Shop.GoodsItem.MainImageId, Shop.GoodsItem.Article, Shop.GoodsItem.Currency, t.Discount
