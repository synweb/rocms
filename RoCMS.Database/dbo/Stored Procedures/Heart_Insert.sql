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
@AdditionalHeaders nvarchar(MAX),
@Type varchar(300),
@Options NVARCHAR(MAX), 
@State VARCHAR(20)
AS
	INSERT INTO [dbo].[Heart] ([RelativeUrl], [ParentHeartId], [BreadcrumbsTitle], [Noindex], [Title], [MetaDescription], [MetaKeywords], [Styles], [Scripts], [Layout], [AdditionalHeaders], [Type], [Options], [State])
	VALUES (@RelativeUrl, @ParentHeartId, @BreadcrumbsTitle, @Noindex, @Title, @MetaDescription, @MetaKeywords, @Styles, @Scripts, @Layout, @AdditionalHeaders, @Type, @Options, @State)
	SELECT @@IDENTITY
