﻿CREATE PROCEDURE [Shop].[Client_Update]
@Email nvarchar(250),
@Phone nvarchar(50),
@EmailNotificationAllowed bit,
@SmsNotificationAllowed bit,
@Name nvarchar(100),
@Comment nvarchar(MAX),
@LastName nvarchar(100),
@Address nvarchar(MAX),
@ClientId int,
@InitialAmount decimal(18,2)
AS
	UPDATE [Shop].[Client] SET
		[Email]=@Email,
		[Phone]=@Phone,
		[EmailNotificationAllowed]=@EmailNotificationAllowed,
		[SmsNotificationAllowed]=@SmsNotificationAllowed,
		[Name]=@Name,
		[Comment]=@Comment,
		[LastName]=@LastName,
		[Address]=@Address,
		[InitialAmount]=@InitialAmount
	WHERE [ClientId]=@ClientId
