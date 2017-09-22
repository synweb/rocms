CREATE PROCEDURE [News].[NewsItem_Delete]
@HeartId int
AS
	DELETE FROM [News].[NewsItem]
	WHERE [HeartId]=@HeartId
