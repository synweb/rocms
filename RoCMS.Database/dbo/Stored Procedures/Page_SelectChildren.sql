CREATE PROCEDURE [dbo].[Page_SelectChildren]
	@ParentPageId int
AS
	SELECT * FROM [Page] WHERE [ParentPageId]=@ParentPageId
