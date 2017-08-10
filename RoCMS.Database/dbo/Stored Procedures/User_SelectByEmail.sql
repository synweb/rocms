CREATE PROCEDURE [dbo].[User_SelectByEmail]
	@Email nvarchar(100)
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]
	WHERE [Email]=@Email
