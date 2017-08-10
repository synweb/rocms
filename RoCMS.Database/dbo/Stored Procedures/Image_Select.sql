CREATE PROCEDURE [dbo].[Image_Select]
AS
	SELECT [ImageId], [CreationDate], [InitialFilename] FROM [dbo].[Image]
