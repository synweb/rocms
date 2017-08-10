CREATE PROCEDURE [dbo].[Image_Update]
	@ImageId varchar(30),
	@InitialFilename nvarchar(257)
AS
	UPDATE Image SET
		[InitialFilename]=@InitialFilename
		WHERE [ImageId]=@ImageId
