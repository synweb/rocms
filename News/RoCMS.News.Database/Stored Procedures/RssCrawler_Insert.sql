CREATE PROCEDURE [News].[RssCrawler_Insert]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategoryId int
AS
	INSERT INTO [News].[RssCrawler] ([RssFeedUrl], [IsEnabled], [CheckInterval], [TargetCategoryId])
	VALUES (@RssFeedUrl, @IsEnabled, @CheckInterval, @TargetCategoryId)
	SELECT @@IDENTITY
