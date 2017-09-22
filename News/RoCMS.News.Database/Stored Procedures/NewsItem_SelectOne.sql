CREATE PROCEDURE [News].[NewsItem_SelectOne]
@HeartId int
AS
	SELECT *
	FROM [News].[NewsItem]
	WHERE [HeartId]=@HeartId
