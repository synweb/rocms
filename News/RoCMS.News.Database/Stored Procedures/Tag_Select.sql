CREATE PROCEDURE [News].[Tag_Select]
AS
	SELECT [TagId], [CreationDate], [Name] FROM [News].[Tag]
