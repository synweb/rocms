CREATE PROCEDURE [dbo].[Heart_Update]
@RelativeUrl nvarchar(300),
@ParentHeartId int,
@BreadcrumbsTitle nvarchar(MAX),
@Noindex bit,
@Title nvarchar(MAX),
@MetaDescription nvarchar(MAX),
@MetaKeywords nvarchar(MAX),
@Styles nvarchar(MAX),
@Scripts nvarchar(MAX),
@Layout varchar(300),
@AdditionalHeaders nvarchar(MAX),
@HeartId int,
@Options NVARCHAR(MAX), 
@State VARCHAR(20)
AS
	UPDATE [dbo].[Heart] SET
		[RelativeUrl]=@RelativeUrl,
		[ParentHeartId]=@ParentHeartId,
		[BreadcrumbsTitle]=@BreadcrumbsTitle,
		[Noindex]=@Noindex,
		[Title]=@Title,
		[MetaDescription]=@MetaDescription,
		[MetaKeywords]=@MetaKeywords,
		[Styles]=@Styles,
		[Scripts]=@Scripts,
		[Layout]=@Layout,
		[AdditionalHeaders]=@AdditionalHeaders,
		[Options]=@Options,
		[State]=@State
	WHERE [HeartId]=@HeartId
