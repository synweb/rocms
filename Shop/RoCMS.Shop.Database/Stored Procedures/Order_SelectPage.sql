CREATE PROCEDURE [Shop].[Order_SelectPage]
	@StartIndex INT,
	@Count INT,
	@TotalCount INT OUTPUT,
	@ClientId INT
AS
	SELECT @TotalCount = COUNT(*) FROM [Shop].[Order] o WHERE @ClientId IS NULL OR o.ClientId = @ClientId

	SELECT * FROM [Shop].[Order] 
	WHERE @ClientId IS NULL OR ClientId = @ClientId
	ORDER BY OrderId DESC
	OFFSET (@StartIndex-1) ROWS
	FETCH NEXT @Count ROWS ONLY 