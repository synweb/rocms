CREATE PROCEDURE [dbo].[User_Select]
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]

