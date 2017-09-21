CREATE PROCEDURE [dbo].[Heart_Insert]
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
@AdditionalHeaders nvarchar(MAX)
AS
	INSERT INTO [dbo].[Heart] ([RelativeUrl], [ParentHeartId], [BreadcrumbsTitle], [Noindex], [Title], [MetaDescription], [MetaKeywords], [Styles], [Scripts], [Layout], [AdditionalHeaders])
	VALUES (@RelativeUrl, @ParentHeartId, @BreadcrumbsTitle, @Noindex, @Title, @MetaDescription, @MetaKeywords, @Styles, @Scripts, @Layout, @AdditionalHeaders)
	SELECT @@IDENTITY
