CREATE PROCEDURE [News].[NewsItem_IncreaseViewCount]
	@HeartId int
AS
	UPDATE [News].[NewsItem] SET 
		[ViewCount]=[ViewCount] + 1
	WHERE [HeartId]=@HeartId
