CREATE PROCEDURE [Shop].[Category_Exists]
	@CategoryId int
AS
	IF EXISTS( SELECT * FROM [Shop].[Category] WHERE [CategoryId]=@CategoryId )
		SELECT 1
	ELSE
		SELECT 0