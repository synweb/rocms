CREATE PROCEDURE [dbo].[Page_Delete]
	@HeartId int
AS
	DELETE FROM [Page] WHERE [HeartId]=@HeartId
