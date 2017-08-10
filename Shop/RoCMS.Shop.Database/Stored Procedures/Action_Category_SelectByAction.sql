CREATE PROCEDURE [Shop].[Action_Category_SelectByAction]
@ActionId int
AS
	SELECT * FROM [Shop].[Action_Category]
	WHERE [ActionId]=@ActionId