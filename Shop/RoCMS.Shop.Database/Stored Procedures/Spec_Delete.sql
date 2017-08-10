CREATE PROCEDURE [Shop].[Spec_Delete]
@SpecId int
AS
	DELETE FROM [Shop].[Spec]
	WHERE [SpecId]=@SpecId
