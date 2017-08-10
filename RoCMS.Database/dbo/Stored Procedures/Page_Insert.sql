CREATE PROCEDURE [dbo].[Page_Insert]
@Title nvarchar(MAX),
@Annotation nvarchar(MAX),
@Content nvarchar(MAX),
@RelativeUrl nvarchar(300),
@Keywords nvarchar(MAX),
@ParentPageId int,
@HideInSitemap bit,
@Header nvarchar(MAX),
@Styles nvarchar(MAX),
@Scripts nvarchar(MAX),
@Layout varchar(300),
@AdditionalHeaders nvarchar(MAX)
AS
	INSERT INTO [dbo].[Page] ([Title], [Annotation], [Content], [RelativeUrl], [Keywords], [ParentPageId], [HideInSitemap], [Header], [Styles], [Scripts], [Layout], [AdditionalHeaders])
	VALUES (@Title, @Annotation, @Content, @RelativeUrl, @Keywords, @ParentPageId, @HideInSitemap, @Header, @Styles, @Scripts, @Layout, @AdditionalHeaders)
	SELECT @@IDENTITY