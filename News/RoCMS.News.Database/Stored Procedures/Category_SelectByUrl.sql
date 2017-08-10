CREATE PROCEDURE [News].[Category_SelectByUrl]
	@RelativeUrl nvarchar(300)
AS
	SELECT * From Category WHERE RelativeUrl = @RelativeUrl
