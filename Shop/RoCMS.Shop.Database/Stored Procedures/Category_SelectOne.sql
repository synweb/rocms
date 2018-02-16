CREATE PROCEDURE [Shop].[Category_SelectOne]
@HeartId int
AS
	SELECT * FROM [Shop].[Category]
	WHERE [HeartId]=@HeartId