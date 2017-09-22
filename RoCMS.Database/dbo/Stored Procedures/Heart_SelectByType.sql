CREATE PROCEDURE [dbo].[Heart_SelectByType]
	@Type varchar(300)
AS
	SELECT * FROM [dbo].[Heart] WHERE [Type]=@Type
