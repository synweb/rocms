CREATE PROCEDURE [Shop].[Client_Delete]
@ClientId int
AS
	DELETE FROM [Shop].[Client]
	WHERE [ClientId]=@ClientId
