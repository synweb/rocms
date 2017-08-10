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

IF (SELECT COUNT(*) FROM [Shop].[Currency]) = 0
BEGIN
	INSERT INTO [Shop].[Currency] (CurrencyId, Name, ShortName, Rate, SortOrder, IsMain) VALUES ('RUR', N'Российский рубль', N'руб.', 1, 0, 1)
END
GO

IF (SELECT COUNT(*) FROM [Shop].[Dimension]) = 0
BEGIN
	SET IDENTITY_INSERT [Shop].[Dimension] ON
		INSERT INTO [Shop].[Dimension] ([DimensionId], [Full], [Short]) VALUES (1, N'Граммы', N'г')
		INSERT INTO [Shop].[Dimension] ([DimensionId], [Full], [Short]) VALUES (2, N'Метры', N'м')
		INSERT INTO [Shop].[Dimension] ([DimensionId], [Full], [Short]) VALUES (3, N'Литры', N'л')
	SET IDENTITY_INSERT [Shop].[Dimension] OFF
END
GO