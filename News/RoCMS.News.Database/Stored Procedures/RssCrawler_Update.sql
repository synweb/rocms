CREATE PROCEDURE [News].[RssCrawler_Update]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategoryId int,
@RssCrawlerId int,
@ImageSelector nvarchar(max)
AS
	UPDATE [News].[RssCrawler] SET
		[RssFeedUrl]=@RssFeedUrl,
		[IsEnabled]=@IsEnabled,
		[CheckInterval]=@CheckInterval,
		[TargetCategoryId]=@TargetCategoryId,
		[ImageSelector]=@ImageSelector
	WHERE [RssCrawlerId]=@RssCrawlerId
