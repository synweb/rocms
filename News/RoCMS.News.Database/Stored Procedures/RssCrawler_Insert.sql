CREATE PROCEDURE [News].[RssCrawler_Insert]
@RssFeedUrl nvarchar(MAX),
@IsEnabled bit,
@CheckInterval int,
@TargetCategoryId int,
@ImageSelector nvarchar(max),
@ContentContainerSelector nvarchar(max),
@LinkText nvarchar(max),
@Tags nvarchar(max)
AS
	INSERT INTO [News].[RssCrawler] ([RssFeedUrl], [IsEnabled], [CheckInterval], [TargetCategoryId], [ImageSelector], [ContentContainerSelector], [LinkText], [Tags])
	VALUES (@RssFeedUrl, @IsEnabled, @CheckInterval, @TargetCategoryId, @ImageSelector, @ContentContainerSelector, @LinkText, @Tags)
	SELECT @@IDENTITY
