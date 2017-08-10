CREATE PROCEDURE [dbo].[Page_SelectOne]
	@PageId int
AS
	SELECT * FROM Page WHERE [PageId]=@PageId
