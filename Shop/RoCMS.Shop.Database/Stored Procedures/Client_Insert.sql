CREATE PROCEDURE [Shop].[Client_Insert]
@Email nvarchar(250),
@Phone nvarchar(50),
@EmailNotificationAllowed bit,
@SmsNotificationAllowed bit,
@UserId int,
@Name nvarchar(100),
@Comment nvarchar(MAX),
@LastName nvarchar(100),
@Address nvarchar(MAX),
@InitialAmount decimal(18,2)
AS
	INSERT INTO [Shop].[Client] ([Email], [Phone], [EmailNotificationAllowed], [SmsNotificationAllowed], [UserId], [Name], [Comment], [LastName], [Address], [InitialAmount])
	VALUES (@Email, @Phone, @EmailNotificationAllowed, @SmsNotificationAllowed, @UserId, @Name, @Comment, @LastName, @Address, @InitialAmount)
	SELECT @@IDENTITY
