CREATE PROCEDURE [Shop].[Client_SelectOneByUserId]
	@UserId int
AS
	SELECT * FROM [Shop].[Client] WHERE UserId=@UserId
