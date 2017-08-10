CREATE PROCEDURE [News].[Blog_SelectOneByRelativeUrl]
	@RelativeUrl nvarchar(200)
AS
	SELECT * FROM [News].Blog WHERE [RelativeUrl]=@RelativeUrl
