CREATE PROCEDURE [dbo].[Slider_SelectOne]
@SliderId int
AS
    SELECT [SliderId], [Name]
    FROM [dbo].[Slider]
    WHERE [SliderId]=@SliderId
