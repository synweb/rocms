CREATE PROCEDURE [Shop].[Goods_GetSpecs]
	@HeartIds [Int_Table] readonly
AS
	SELECT DISTINCT SpecId AS [Key], Value FROM [Shop].Goods_Spec WHERE HeartId IN (SELECT * FROM @HeartIds)
