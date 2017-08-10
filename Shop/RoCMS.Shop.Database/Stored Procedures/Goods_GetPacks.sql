CREATE PROCEDURE [Shop].[Goods_GetPacks]
@GoodsIds [Int_Table] readonly
AS
	SELECT DISTINCT gp.PackId AS ID, p.Name FROM [Shop].Goods_Pack gp JOIN Pack p ON gp.PackId=p.PackId WHERE GoodsId IN (SELECT * FROM @GoodsIds)