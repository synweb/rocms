CREATE PROCEDURE [dbo].[Block_SelectByName]
	@Name nvarchar(200)
AS
	SELECT * FROM [dbo].[Block] WHERE [Name]=@Name
