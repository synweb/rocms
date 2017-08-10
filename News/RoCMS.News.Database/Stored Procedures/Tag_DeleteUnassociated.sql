CREATE PROCEDURE [News].[Tag_DeleteUnassociated]
AS
	DELETE FROM [News].[Tag] WHERE TagId NOT IN (SELECT TagId FROM [News].[NewsItemTag])
