CREATE PROCEDURE [Shop].[Goods_Spec_Delete]
@HeartId int,
@SpecId int
AS
	DELETE FROM [Shop].[Goods_Spec]
	WHERE [HeartId]=@HeartId
		 AND [SpecId]=@SpecId
