CREATE PROCEDURE [dbo].[Block_Update]
@BlockId int,
@Title nvarchar(max),
@Content nvarchar(max),
@Name nvarchar(200)
AS
	UPDATE [dbo].[Block]
	SET [Title]=@Title, 
		[Content]=@Content,
		[Name]=@Name
	WHERE [BlockId]=@BlockId
