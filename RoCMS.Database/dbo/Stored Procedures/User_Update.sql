CREATE PROCEDURE [dbo].[User_Update]
@Email nvarchar(100),
@EmailConfirmed bit,
@Description nvarchar(1000),
@Vk nvarchar(100),
@Fb nvarchar(100),
@GoogleP nvarchar(100),
@Twitter nvarchar(100),
@UserId int
AS
	UPDATE [User] SET
		[Email]=@Email,
		[EmailConfirmed]=@EmailConfirmed,
		[Description]=@Description,
		[Vk]=@Vk,
		[Fb]=@Fb,
		[GoogleP]=@GoogleP,
		[Twitter]=@Twitter
		WHERE [UserId]=@UserId
