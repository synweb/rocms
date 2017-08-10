CREATE PROCEDURE [dbo].[Image_Delete]
@ImageId varchar(30)
AS
	DELETE FROM [dbo].[Image]
	WHERE [ImageId]=@ImageId
