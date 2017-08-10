CREATE PROCEDURE [dbo].[Image_CheckIfExists]
	@ImageId varchar(30)
AS
	IF EXISTS( SELECT * FROM Image WHERE [ImageId]=@ImageId )
		SELECT 1
	ELSE
		SELECT 0
