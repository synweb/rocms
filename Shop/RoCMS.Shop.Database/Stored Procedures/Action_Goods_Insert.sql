CREATE PROCEDURE [Shop].[Action_Goods_Insert]
@ActionId int,
@GoodsId int
AS
	INSERT INTO [Shop].[Action_Goods] ([ActionId], [GoodsId])
	VALUES (@ActionId, @GoodsId)
