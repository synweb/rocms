CREATE PROCEDURE [dbo].[Country_SelectOne]
	@CountryId int
AS
	SELECT * FROM [dbo].[Country] WHERE [CountryId]=@CountryId
