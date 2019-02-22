CREATE PROCEDURE [dbo].[FormRequest_SelectOne]
	@FormRequestId INT
AS
	SELECT *
	FROM [dbo].[FormRequest]
	WHERE [FormRequestId] = @FormRequestId
