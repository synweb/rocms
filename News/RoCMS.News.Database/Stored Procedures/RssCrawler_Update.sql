CREATE PROCEDURE [News].[RssCrawler_Update]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategoryId int,
@RssCrawlerId int
AS
	UPDATE [News].[RssCrawler] SET
		[RssFeedUrl]=@RssFeedUrl,
		[IsEnabled]=@IsEnabled,
		[CheckInterval]=@CheckInterval,
		[TargetCategoryId]=@TargetCategoryId
	WHERE [RssCrawlerId]=@RssCrawlerId
