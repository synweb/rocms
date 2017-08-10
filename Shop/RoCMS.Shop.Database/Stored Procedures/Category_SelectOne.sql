CREATE PROCEDURE [Shop].[Category_SelectOne]
@CategoryId int
AS
	SELECT * FROM [Shop].[Category]
	WHERE [CategoryId]=@CategoryId