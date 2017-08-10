CREATE PROCEDURE [dbo].[Page_Update]
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
@AdditionalHeaders nvarchar(MAX),
@PageId int
AS
	UPDATE [dbo].[Page] SET
		[Title]=@Title,
		[Annotation]=@Annotation,
		[Content]=@Content,
		[RelativeUrl]=@RelativeUrl,
		[Keywords]=@Keywords,
		[ParentPageId]=@ParentPageId,
		[HideInSitemap]=@HideInSitemap,
		[Header]=@Header,
		[Styles]=@Styles,
		[Scripts]=@Scripts,
		[Layout]=@Layout,
		[AdditionalHeaders]=@AdditionalHeaders
	WHERE [PageId]=@PageId