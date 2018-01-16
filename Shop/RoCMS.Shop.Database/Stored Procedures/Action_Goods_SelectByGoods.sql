CREATE PROCEDURE [Shop].[Action_Goods_SelectByGoods]
@HeartId int
AS
	SELECT * FROM [Shop].[Action_Goods]
		WHERE [HeartId]=@HeartId
