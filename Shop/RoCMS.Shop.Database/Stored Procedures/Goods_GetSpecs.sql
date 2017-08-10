CREATE PROCEDURE [Shop].[Goods_GetSpecs]
	@GoodsIds [Int_Table] readonly
AS
	SELECT DISTINCT SpecId AS [Key], Value FROM [Shop].Goods_Spec WHERE GoodsId IN (SELECT * FROM @GoodsIds)
