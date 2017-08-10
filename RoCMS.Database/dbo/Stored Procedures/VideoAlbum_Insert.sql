CREATE PROCEDURE [dbo].[VideoAlbum_Insert]
@Name  NVARCHAR (50),
@CreationDate DATETIME
/*,@OwnerId INT*/
AS
	INSERT INTO [dbo].[VideoAlbum] ([Name], [CreationDate])
	VALUES (@Name, @CreationDate)
	SELECT @@IDENTITY