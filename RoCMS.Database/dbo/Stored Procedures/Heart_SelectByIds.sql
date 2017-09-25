CREATE PROCEDURE [dbo].[Heart_SelectByIds]
	@HeartIds [Int_Table] readonly
AS
	SELECT * FROM Heart WHERE HeartId IN (Select Val From @HeartIds)
