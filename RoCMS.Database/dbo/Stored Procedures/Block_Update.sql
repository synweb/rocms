CREATE PROCEDURE [dbo].[Block_Update]
@BlockId int,
@Title nvarchar(max),
@Content nvarchar(max)
AS
	UPDATE [dbo].[Block]
	SET [Title]=@Title, 
		[Content]=@Content
	WHERE [BlockId]=@BlockId
