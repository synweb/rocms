CREATE PROCEDURE [dbo].[User_UpdatePassword]
    @UserId int,
    @PassHash nvarchar(200)
AS
	UPDATE [dbo].[User] SET [Password] = @PassHash WHERE [UserId]=@UserId
