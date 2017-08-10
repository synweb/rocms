CREATE PROCEDURE [Shop].[GoodsItem_Exists]
	@GoodsId int
AS
	IF EXISTS( SELECT * FROM [Shop].[GoodsItem] 
	WHERE [GoodsId]=@GoodsId AND [Deleted]=0)
		SELECT 1
	ELSE
		SELECT 0