CREATE PROCEDURE [dbo].[PasswordTicket_Insert]
	@UserId int,
	@Token uniqueidentifier,
	@ExpirationDate datetime
AS
	INSERT INTO [PasswordTicket] (UserId,Token,ExpirationDate) VALUES (@UserId, @Token, @ExpirationDate)
	SELECT @@IDENTITY