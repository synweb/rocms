CREATE PROCEDURE [dbo].[Page_SelectOne]
	@HeartId int
AS
	SELECT * FROM [Page] WHERE [HeartId]=@HeartId
