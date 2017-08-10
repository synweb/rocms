CREATE PROCEDURE [dbo].[Page_Delete]
	@PageId int
AS
	DELETE FROM [Page] WHERE [PageId]=@PageId
