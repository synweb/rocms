CREATE PROCEDURE [dbo].[Page_SelectByRelativeUrl]
	@RelativeUrl nvarchar(300)
AS
	SELECT * FROM [Page] WHERE [RelativeUrl]=@RelativeUrl
