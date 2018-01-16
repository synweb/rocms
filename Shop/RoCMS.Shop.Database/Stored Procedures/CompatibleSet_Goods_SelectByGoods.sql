CREATE PROCEDURE [Shop].[CompatibleSet_Goods_SelectByGoods]
@HeartId int
AS
	SELECT * FROM [Shop].[CompatibleSet_Goods]
		WHERE HeartId=@HeartId
