CREATE PROCEDURE [dbo].[Slide_Select]
@SliderId INT
AS
    SELECT [SlideId], [SliderId], [Title], [Description], [ImageId], [Link], [SortOrder]
    FROM [dbo].[Slide]
	WHERE [SliderId]=@SliderId