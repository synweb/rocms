CREATE PROCEDURE [dbo].[Heart_Delete]
@HeartId int
AS
	DELETE FROM [dbo].[Heart]
	WHERE [HeartId]=@HeartId
