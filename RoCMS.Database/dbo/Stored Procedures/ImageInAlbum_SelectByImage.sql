CREATE PROCEDURE [dbo].[ImageInAlbum_SelectByImage]
	@ImageId varchar(30)
AS
	SELECT * FROM [ImageInAlbum] WHERE [ImageId]=@ImageId