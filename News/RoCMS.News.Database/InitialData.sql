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

-- Если нет блогов
IF NOT EXISTS (SELECT * FROM [News].[Blog])
BEGIN
    DECLARE @adminId INT

    SELECT @adminId=UserId FROM [dbo].[User] WHERE Username='admin'

    SET IDENTITY_INSERT [News].[Blog] ON
    INSERT INTO [News].[Blog] ([BlogId], [Title], [OwnerId]) VALUES 
        (1, N'Новости сайта', @adminId)
    INSERT INTO [News].[Blog_User] ([BlogId],[UserId]) VALUES (1, @adminId) 
    SET IDENTITY_INSERT [News].[Blog] OFF
END
GO