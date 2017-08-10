CREATE PROCEDURE [dbo].[PasswordTicket_UseTicket]
	@TicketId int
AS
	UPDATE PasswordTicket SET
		[Used]=1,
		[UseDate]=GETUTCDATE()
		WHERE [TicketId]=@TicketId
