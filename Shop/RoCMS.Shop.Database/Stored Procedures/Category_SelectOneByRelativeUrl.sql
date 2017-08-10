CREATE PROCEDURE [Shop].[Category_SelectOneByRelativeUrl]
@RelativeUrl nvarchar(300)
AS
	SELECT * FROM [Shop].[Category]
	WHERE [RelativeUrl]=@RelativeUrl