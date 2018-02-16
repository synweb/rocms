CREATE PROCEDURE [Shop].[Category_Delete]
@HeartId int
AS
	DELETE FROM [Shop].[Category]
	WHERE [HeartId]=@HeartId
