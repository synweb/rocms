CREATE PROCEDURE [News].[RssCrawler_Update]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategory int,
@RssCrawlerId int
AS
	UPDATE [News].[RssCrawler] SET
		[RssFeedUrl]=@RssFeedUrl,
		[IsEnabled]=@IsEnabled,
		[CheckInterval]=@CheckInterval,
		[TargetCategory]=@TargetCategory
	WHERE [RssCrawlerId]=@RssCrawlerId
