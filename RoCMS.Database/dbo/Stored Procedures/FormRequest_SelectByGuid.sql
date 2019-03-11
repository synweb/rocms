CREATE PROCEDURE [dbo].[FormRequest_SelectByGuid]
	@Guid UNIQUEIDENTIFIER
AS
	SELECT * FROM [dbo].[FormRequest] WHERE [Guid]=@Guid
