CREATE PROCEDURE [Shop].[Category_Exists]
	@HeartId int
AS
	IF EXISTS( SELECT * FROM [Shop].[Category] WHERE [HeartId]=@HeartId )
		SELECT 1
	ELSE
		SELECT 0