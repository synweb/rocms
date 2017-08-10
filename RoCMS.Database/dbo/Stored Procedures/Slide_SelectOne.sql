CREATE PROCEDURE [dbo].[Slide_SelectOne]
@SlideId INT
AS
    SELECT [SlideId], [SliderId], [Title], [Description], [ImageId], [Link], [SortOrder]
    FROM [dbo].[Slide]
    WHERE [SlideId]=@SlideId
