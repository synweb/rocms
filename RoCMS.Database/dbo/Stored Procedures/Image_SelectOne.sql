CREATE PROCEDURE [dbo].[Image_SelectOne]
	@ImageId varchar(30)
AS
	SELECT * FROM Image WHERE ImageId=@ImageId
