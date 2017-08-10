CREATE PROCEDURE [dbo].[FormRequest_Delete]
	@FormRequestId INT
AS
	DELETE FROM [dbo].[FormRequest]
	WHERE [FormRequestId] = @FormRequestId
