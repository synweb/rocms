CREATE PROCEDURE [dbo].[FormRequest_Select]
AS
	SELECT *
	FROM [dbo].[FormRequest]
	ORDER BY [CreationDate] DESC
