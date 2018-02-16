CREATE PROCEDURE [Shop].[GoodsItem_SelectOne]
@HeartId int
AS
	SELECT * FROM [Shop].[GoodsItem]
	WHERE [HeartId]=@HeartId AND Deleted=0
