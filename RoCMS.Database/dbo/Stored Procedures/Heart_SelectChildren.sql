CREATE PROCEDURE [dbo].[Heart_SelectChildren]
	@ParentHeartId int
AS
	SELECT * FROM [Heart] WHERE [ParentHeartId]=@ParentHeartId
