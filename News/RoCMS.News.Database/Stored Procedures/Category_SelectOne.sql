CREATE PROCEDURE [News].[Category_SelectOne]
	@CategoryId int
AS
	SELECT * FROM [News].[Category]
		WHERE CategoryId=@CategoryId
