CREATE PROCEDURE [Shop].[Action_Goods_SelectByAction]
@ActionId int
AS
	SELECT * FROM [Shop].[Action_Goods]
		WHERE [ActionId]=@ActionId
