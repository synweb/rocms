CREATE PROCEDURE [dbo].[Slider_Insert]
@Name nvarchar(50)
AS
    INSERT INTO [dbo].[Slider] ([Name])
    VALUES (@Name)
    SELECT @@IDENTITY
