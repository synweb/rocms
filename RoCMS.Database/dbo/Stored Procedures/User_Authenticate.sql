CREATE PROCEDURE [dbo].[User_Authenticate]
    @Username nvarchar(50), 
    @PassHash nvarchar(200)
AS
	IF (SELECT COUNT(*) FROM [dbo].[User] WHERE LOWER(Username)=LOWER(@Username) AND Password=@PassHash) = 0
		SELECT 0
	ELSE
		SELECT 1
