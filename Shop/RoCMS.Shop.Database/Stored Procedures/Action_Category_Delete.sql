CREATE PROCEDURE [Shop].[Action_Category_Delete]
@ActionId int,
@CategoryId int
AS
	DELETE FROM [Shop].[Action_Category]
	WHERE [ActionId]=@ActionId
		 AND [CategoryId]=@CategoryId
