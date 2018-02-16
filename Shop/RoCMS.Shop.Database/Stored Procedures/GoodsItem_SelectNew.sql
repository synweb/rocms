CREATE PROCEDURE [Shop].[GoodsItem_SelectNew]
	@Count int
AS
	SELECT TOP (@Count) * FROM [Shop].[GoodsItem] WHERE
		Deleted=0 AND NotAvailable=0 
		AND MainImageId IS NOT NULL AND MainImageId != ''
	ORDER BY HeartId DESC
