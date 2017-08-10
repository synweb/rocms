CREATE PROCEDURE [Shop].[Client_SelectPage]
	@StartIndex INT,
	@Count INT,
	@TotalCount INT OUTPUT
AS
	SELECT @TotalCount = COUNT(*) FROM [Shop].[Client]

	SELECT * FROM [Shop].[Client]
	ORDER BY ClientId DESC
	OFFSET (@StartIndex-1) ROWS
	FETCH NEXT @Count ROWS ONLY 