CREATE PROCEDURE [Shop].[Action_Goods_Delete]
@ActionId int,
@HeartId int
AS
	DELETE FROM [Shop].[Action_Goods]
	WHERE [ActionId]=@ActionId
		 AND [HeartId]=@HeartId
