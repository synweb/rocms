CREATE PROCEDURE [News].[RssCrawler_Update]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategoryId int,
@RssCrawlerId int,
@ImageSelector nvarchar(max),
@ContentContainerSelector nvarchar(max),
@LinkText nvarchar(max),
@Tags nvarchar(max),
@ExcludeTags nvarchar(max)
AS
	UPDATE [News].[RssCrawler] SET
		[RssFeedUrl]=@RssFeedUrl,
		[IsEnabled]=@IsEnabled,
		[CheckInterval]=@CheckInterval,
		[TargetCategoryId]=@TargetCategoryId,
		[ImageSelector]=@ImageSelector,
		[ContentContainerSelector]=@ContentContainerSelector,
		[LinkText]=@LinkText,
		[Tags]=@Tags,
		[ExcludeTags]=@ExcludeTags
	WHERE [RssCrawlerId]=@RssCrawlerId
