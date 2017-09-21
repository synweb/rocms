CREATE PROCEDURE [dbo].[Page_Update]
@HeartId int,
@Content nvarchar(MAX)
AS
	UPDATE [dbo].[Page] SET
		[Content]=@Content
	WHERE [HeartId]=@HeartId
