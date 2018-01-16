CREATE PROCEDURE [Shop].[Goods_Pack_SelectByGoods]
@HeartId int
AS
	SELECT * FROM [Shop].[Goods_Pack]
		WHERE [HeartId]=@HeartId