CREATE PROCEDURE [Shop].[Goods_Spec_SelectByGoods]
@HeartId int
AS
	SELECT * FROM [Shop].[Goods_Spec]
		WHERE [HeartId]=@HeartId