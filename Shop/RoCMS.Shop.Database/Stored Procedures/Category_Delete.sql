CREATE PROCEDURE [Shop].[Category_Delete]
@CategoryId int
AS
	DELETE FROM [Shop].[Category]
	WHERE [CategoryId]=@CategoryId
