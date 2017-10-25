CREATE PROCEDURE [Comments].[Comment_SelectHeartIds]
	@StartIndex INT,
	@Count INT,
	@TotalCount INT OUTPUT
AS

DECLARE @HeartIds TABLE(HeartId INT PRIMARY KEY, LastCommentDate DATETIME)

INSERT INTO @HeartIds (HeartId, LastCommentDate)
SELECT HeartId, CreationDate
	FROM Comments.Comment c
	WHERE CreationDate = (SELECT MAX(CreationDate) FROM Comments.Comment WHERE HeartId = c.HeartId)

SELECT @TotalCount = COUNT([HeartId]) FROM @HeartIds

SELECT HeartId FROM @HeartIds
ORDER BY LastCommentDate DESC
OFFSET (@StartIndex-1) ROWS
FETCH NEXT @Count ROWS ONLY