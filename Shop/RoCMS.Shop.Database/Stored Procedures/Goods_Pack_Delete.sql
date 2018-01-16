CREATE PROCEDURE [Shop].[Goods_Pack_Delete]
@PackId int,
@HeartId int
AS
	DELETE FROM [Shop].[Goods_Pack]
	WHERE [PackId]=@PackId
		 AND [HeartId]=@HeartId
