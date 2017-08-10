/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

DECLARE @resourceId INT
IF EXISTS (SELECT * FROM [dbo].[CmsResource] WHERE [Name] = N'FAQ')
	SELECT @resourceId = [CmsResourceId] FROM [dbo].[CmsResource] WHERE [Name] = N'FAQ'
ELSE
BEGIN
	INSERT INTO [dbo].[CmsResource] ([Name], [Description]) VALUES (N'FAQ', N'Вопросы')
	SET @resourceId = @@IDENTITY
END

