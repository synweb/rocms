CREATE PROCEDURE [Shop].[GoodsItem_Exists]
	@HeartId int
AS
	IF EXISTS( SELECT * FROM [Shop].[GoodsItem] 
	WHERE [HeartId]=@HeartId AND [Deleted]=0)
		SELECT 1
	ELSE
		SELECT 0