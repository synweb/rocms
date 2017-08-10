CREATE PROCEDURE [dbo].[PasswordTicket_SelectAvailableByToken]
	@Token uniqueidentifier
AS
	SELECT * FROM [PasswordTicket] 
		WHERE [Token]=@Token AND [ExpirationDate]>=GETUTCDATE() AND [Used]=0
