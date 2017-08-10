CREATE PROCEDURE [dbo].[Slide_Update]
@SlideId    INT,
@SliderId    INT,
@Title       NVARCHAR (50),
@Description NVARCHAR (1000),
@ImageId     VARCHAR(30),
@Link        NVARCHAR (MAX)
AS
    UPDATE [dbo].[Slide]
    SET [SliderId]    =@SliderId,
        [Title]       =@Title,
        [Description] =@Description,
        [ImageId]     =@ImageId,
        [Link]        =@Link
    WHERE [SlideId]=@SlideId
