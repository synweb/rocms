CREATE PROCEDURE [Shop].[GoodsInOrder_SelectByGoods]
@HeartId int
AS
	SELECT * FROM [Shop].[GoodsInOrder]
		WHERE [HeartId]=@HeartId