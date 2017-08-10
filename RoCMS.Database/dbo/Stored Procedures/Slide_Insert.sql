CREATE PROCEDURE [dbo].[Slide_Insert]
@SliderId    INT,
@Title       NVARCHAR (50),
@Description NVARCHAR (1000),
@ImageId     VARCHAR(30),
@Link        NVARCHAR (MAX)
AS
    INSERT INTO [dbo].[Slide] ([SliderId], [Title], [Description], [ImageId], [Link])
    VALUES (@SliderId, @Title, @Description, @ImageId, @Link)
    SELECT @@IDENTITY
