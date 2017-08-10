CREATE PROCEDURE [News].[Tag_Delete]
@TagId int
AS
	DELETE FROM [News].[Tag]
	WHERE [TagId]=@TagId
