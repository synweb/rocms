CREATE PROCEDURE [Shop].[Client_UpdateInfo]

@Phone nvarchar(50),
@EmailNotificationAllowed bit,
@SmsNotificationAllowed bit,

@Name nvarchar(100),

@LastName nvarchar(100),
@Address nvarchar(MAX),
@ClientId int

AS
	UPDATE [Shop].[Client] SET

		[Phone]=@Phone,
		[EmailNotificationAllowed]=@EmailNotificationAllowed,
		[SmsNotificationAllowed]=@SmsNotificationAllowed,

		[Name]=@Name,

		[LastName]=@LastName,
		[Address]=@Address

	WHERE [ClientId]=@ClientId
