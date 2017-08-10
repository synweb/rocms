CREATE PROCEDURE [dbo].[Block_SelectOne]
@BlockId int
AS
	SELECT [BlockId], [Title], [Content] FROM [dbo].[Block]
	WHERE [BlockId]=@BlockId
