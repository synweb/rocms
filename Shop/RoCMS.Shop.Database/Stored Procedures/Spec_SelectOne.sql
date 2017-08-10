CREATE PROCEDURE [Shop].[Spec_SelectOne]
@SpecId int
AS
	SELECT * FROM [Shop].[Spec] WHERE [SpecId]=@SpecId
