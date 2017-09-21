CREATE PROCEDURE [dbo].[Heart_SelectByRelativeUrl]
	@RelativeUrl nvarchar(300)
AS
	SELECT * FROM [Heart] WHERE [RelativeUrl]=@RelativeUrl
