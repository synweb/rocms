CREATE PROCEDURE [dbo].[Slide_Delete]
@SlideId int
AS
    DELETE FROM [dbo].[Slide]
    WHERE [SlideId]=@SlideId    
