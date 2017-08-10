CREATE PROCEDURE [Shop].[Action_Goods_Delete]
@ActionId int,
@GoodsId int
AS
	DELETE FROM [Shop].[Action_Goods]
	WHERE [ActionId]=@ActionId
		 AND [GoodsId]=@GoodsId
