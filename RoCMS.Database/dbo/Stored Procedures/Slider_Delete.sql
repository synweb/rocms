CREATE PROCEDURE [dbo].[Slider_Delete]
@SliderId int
AS
    DELETE FROM [dbo].[Slider]
    WHERE [SliderId]=@SliderId
