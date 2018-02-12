CREATE PROCEDURE [News].[RssCrawler_Insert]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategory int
AS
	INSERT INTO [News].[RssCrawler] ([RssFeedUrl], [IsEnabled], [CheckInterval], [TargetCategory])
	VALUES (@RssFeedUrl, @IsEnabled, @CheckInterval, @TargetCategory)
	SELECT @@IDENTITY
