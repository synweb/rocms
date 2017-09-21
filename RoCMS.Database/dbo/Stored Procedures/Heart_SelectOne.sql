CREATE PROCEDURE [dbo].[Heart_SelectOne]
@HeartId int
AS
	SELECT * FROM [dbo].[Heart]
	WHERE [HeartId]=@HeartId
