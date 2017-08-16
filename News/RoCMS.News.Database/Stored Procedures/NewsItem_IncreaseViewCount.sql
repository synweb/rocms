CREATE PROCEDURE [News].[NewsItem_IncreaseViewCount]
	@NewsId int
AS
	UPDATE [News].[NewsItem] SET 
		[ViewCount]=[ViewCount] + 1
	WHERE [NewsId]=@NewsId
