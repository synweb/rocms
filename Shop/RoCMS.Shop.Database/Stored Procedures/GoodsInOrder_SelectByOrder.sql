CREATE PROCEDURE [Shop].[GoodsInOrder_SelectByOrder]
@OrderId int
AS
	SELECT * FROM [Shop].[GoodsInOrder]
		WHERE [OrderId]=@OrderId