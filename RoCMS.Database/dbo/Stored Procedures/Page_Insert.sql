CREATE PROCEDURE [dbo].[Page_Insert]
@HeartId int,
@Content nvarchar(MAX)
AS
	INSERT INTO [dbo].[Page] ([HeartId], [Content])
	VALUES (@HeartId, @Content)
	SELECT @HeartId