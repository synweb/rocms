CREATE PROCEDURE [dbo].[Slider_Update]
@SliderId int,
@Name nvarchar(50)
AS
    UPDATE [dbo].[Slider]
    SET [Name]=@Name
    WHERE [SliderId]=@SliderId    
