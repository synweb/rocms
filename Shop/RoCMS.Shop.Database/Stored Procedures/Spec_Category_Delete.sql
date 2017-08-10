CREATE PROCEDURE [Shop].[Spec_Category_Delete]
@CategoryId int,
@SpecId int
AS
	DELETE FROM [Shop].[Spec_Category]
	WHERE [CategoryId]=@CategoryId
		 AND [SpecId]=@SpecId
