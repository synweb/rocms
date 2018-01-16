CREATE PROCEDURE [Shop].[Action_Goods_Insert]
@ActionId int,
@HeartId int
AS
	INSERT INTO [Shop].[Action_Goods] ([ActionId], [HeartId])
	VALUES (@ActionId, @HeartId)
