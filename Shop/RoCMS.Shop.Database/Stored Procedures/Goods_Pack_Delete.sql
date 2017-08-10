CREATE PROCEDURE [Shop].[Goods_Pack_Delete]
@PackId int,
@GoodsId int
AS
	DELETE FROM [Shop].[Goods_Pack]
	WHERE [PackId]=@PackId
		 AND [GoodsId]=@GoodsId
