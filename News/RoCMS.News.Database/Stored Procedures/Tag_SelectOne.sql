CREATE PROCEDURE [News].[Tag_SelectOne]
@TagId int
AS
	SELECT [TagId], [CreationDate], [Name] FROM [News].[Tag]
	WHERE [TagId]=@TagId
