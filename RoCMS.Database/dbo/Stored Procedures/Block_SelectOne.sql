CREATE PROCEDURE [dbo].[Block_SelectOne]
@BlockId int
AS
	SELECT * FROM [dbo].[Block]
	WHERE [BlockId]=@BlockId
