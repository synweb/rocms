CREATE FULLTEXT INDEX
	ON [Shop].[GoodsItem]
		([Name] Language 1049, [HtmlDescription] Language 1049)
	KEY INDEX [PK_Goods]
	ON [Shop_GoodsCatalog]
	WITH CHANGE_TRACKING AUTO
