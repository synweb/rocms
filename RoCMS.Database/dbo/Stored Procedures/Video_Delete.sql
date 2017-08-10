CREATE PROCEDURE [dbo].[Video_Delete]
@VideoId varchar(50)
AS
	DELETE FROM [dbo].[Video]
	WHERE [VideoId]=@VideoId
