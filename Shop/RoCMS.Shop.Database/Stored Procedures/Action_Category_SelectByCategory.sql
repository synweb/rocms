CREATE PROCEDURE [Shop].[Action_Category_SelectByCategory]
@CategoryId int
AS
	SELECT * FROM [Shop].[Action_Category]
	WHERE [CategoryId]=@CategoryId