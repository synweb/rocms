CREATE PROCEDURE [Shop].[GoodsItem_Select]
AS
	SELECT * FROM [Shop].[GoodsItem]
	WHERE Deleted=0
