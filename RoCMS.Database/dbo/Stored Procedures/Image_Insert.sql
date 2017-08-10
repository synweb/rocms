CREATE PROCEDURE [dbo].[Image_Insert]
@ImageId varchar(30),
@InitialFilename nvarchar(257)
AS
	INSERT INTO [dbo].[Image] ([ImageId], [InitialFilename])
	VALUES (@ImageId, @InitialFilename)
