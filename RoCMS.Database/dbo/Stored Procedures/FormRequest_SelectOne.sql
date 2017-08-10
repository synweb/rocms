CREATE PROCEDURE [dbo].[FormRequest_SelectOne]
	@FormRequestId INT
AS
	SELECT [FormRequestId], [Name], [Email], [Phone], [Text], [CreationDate]
	FROM [dbo].[FormRequest]
	WHERE [FormRequestId] = @FormRequestId
