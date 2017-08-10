CREATE PROCEDURE [dbo].[Review_Insert]
@Author    NVARCHAR (50), 
@City      NVARCHAR (50), 
@Email     NVARCHAR (100),
@Text      NVARCHAR (MAX),	
@Response      NVARCHAR (MAX),	
@Moderated BIT,           	
@VK 	   NVARCHAR(50)
AS
	INSERT INTO [dbo].[Review] ([Author], [City], [Email], [Text], [Moderated], [VK], [Response] )
	VALUES (@Author, @City, @Email, @Text, @Moderated, @VK, @Response)
	SELECT @@IDENTITY
