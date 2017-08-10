CREATE PROCEDURE [dbo].[Block_Delete]
@BlockId int
AS
	DELETE FROM [dbo].[Block]
	WHERE [BlockId]=@BlockId
