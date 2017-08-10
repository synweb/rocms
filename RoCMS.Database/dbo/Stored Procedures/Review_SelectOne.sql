CREATE PROCEDURE [dbo].[Review_SelectOne]
@ReviewId INT
AS
	SELECT *
	FROM [dbo].[Review]
	WHERE [ReviewId] = @ReviewId
