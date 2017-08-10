CREATE PROCEDURE [dbo].[User_SelectByUsername]
	@Username nvarchar(50)
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]
	WHERE [Username]=@Username
