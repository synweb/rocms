CREATE PROCEDURE [Shop].[Client_SelectOne]
@ClientId int
AS
	SELECT * FROM [Shop].[Client]
	WHERE [ClientId]=@ClientId
