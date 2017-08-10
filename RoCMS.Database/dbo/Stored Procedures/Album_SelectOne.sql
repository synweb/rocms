CREATE PROCEDURE [dbo].[Album_SelectOne]
	@AlbumId int
AS
	SELECT * FROM [Album] WHERE [AlbumId]=@AlbumId
