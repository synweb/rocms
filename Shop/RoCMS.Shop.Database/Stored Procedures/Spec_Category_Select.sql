CREATE PROCEDURE [Shop].[Spec_Category_SelectByCategory]
@CategoryId int
AS
	SELECT * FROM [Shop].[Spec_Category]
		where [CategoryId]=@CategoryId
