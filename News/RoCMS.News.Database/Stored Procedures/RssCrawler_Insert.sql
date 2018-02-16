CREATE PROCEDURE [News].[RssCrawler_Insert]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategoryId int,
@ImageSelector nvarchar(max)
AS
	INSERT INTO [News].[RssCrawler] ([RssFeedUrl], [IsEnabled], [CheckInterval], [TargetCategoryId], [ImageSelector])
	VALUES (@RssFeedUrl, @IsEnabled, @CheckInterval, @TargetCategoryId, @ImageSelector)
	SELECT @@IDENTITY
