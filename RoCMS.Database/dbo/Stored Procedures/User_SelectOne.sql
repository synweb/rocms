CREATE PROCEDURE [dbo].[User_SelectOne]
	@UserId int
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]
	WHERE [UserId]=@UserId

