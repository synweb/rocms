CREATE PROCEDURE [dbo].[FormRequest_Select]
AS
	SELECT [FormRequestId], [Name], [Email], [Phone], [Text], [CreationDate]
	FROM [dbo].[FormRequest]
	ORDER BY [CreationDate] DESC
