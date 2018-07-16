CREATE PROCEDURE [News].[NewsItem_SelectIdByRssSource]
	@RssSource nvarchar(max)
AS
	SELECT HeartId FROM [News].[NewsItem] where [RssSource]=@RssSource
