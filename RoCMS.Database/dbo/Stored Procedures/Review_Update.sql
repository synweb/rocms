CREATE PROCEDURE [dbo].[Review_Update]
@ReviewId INT,
@Author    NVARCHAR (50), 
@City      NVARCHAR (50), 
@Email     NVARCHAR (100),
@Text      NVARCHAR (MAX),	
@Moderated BIT,           	
@VK 	   NVARCHAR(50),
@Response      NVARCHAR (MAX)
AS
	UPDATE [dbo].[Review]
	set [Author] = @Author,
        [City] = @City,
		[Email] = @Email,
		[Text] = @Text, 
		[Moderated] = @Moderated,
		[VK] = @VK,
		[Response]=@Response
    WHERE [ReviewId] = @ReviewId