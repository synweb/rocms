CREATE PROCEDURE [Shop].[GoodsInOrder_Delete]
@Id int
AS
	DELETE FROM [Shop].[GoodsInOrder]
	WHERE [Id]=@Id
