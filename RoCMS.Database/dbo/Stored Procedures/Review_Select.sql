CREATE PROCEDURE [dbo].[Review_Select]
@Start INT,
@Count INT,
@Total INT OUT,
@OnlyModerated BIT
AS
    SELECT @Total = COUNT(*) 
	FROM [dbo].[Review]
	WHERE [Moderated] = 1

	SELECT *
	FROM [dbo].[Review]
	WHERE @OnlyModerated = 0 or [Moderated] = 1
	ORDER BY [CreationDate] DESC
	OFFSET @Start - 1   ROWS
	FETCH NEXT @Count ROWS ONLY