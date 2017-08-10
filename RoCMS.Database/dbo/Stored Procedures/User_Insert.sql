CREATE PROCEDURE [dbo].[User_Insert]
@Username nvarchar(50),
@Password nvarchar(200),
@Email nvarchar(100),
@EmailConfirmed bit,
@Description nvarchar(1000),
@Vk nvarchar(100),
@Fb nvarchar(100),
@GoogleP nvarchar(100),
@Twitter nvarchar(100)
AS
	INSERT INTO [dbo].[User] ([Username], [Password], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter])
	VALUES (@Username, @Password, @Email, @EmailConfirmed, @Description, @Vk, @Fb, @GoogleP, @Twitter)
	SELECT @@IDENTITY
