CREATE PROCEDURE [dbo].[User_Delete]
@UserId int
AS
	DELETE FROM [dbo].[User]
	WHERE [UserId]=@UserId
