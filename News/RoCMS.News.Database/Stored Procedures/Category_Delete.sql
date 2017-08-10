CREATE PROCEDURE [News].[Category_Delete]
@CategoryId int
AS
	DELETE FROM [News].[Category]
	WHERE [CategoryId]=@CategoryId
