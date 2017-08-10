CREATE PROCEDURE [dbo].[Review_Delete]
@ReviewId int
AS
	DELETE FROM [dbo].[Review]
	WHERE [ReviewId]=@ReviewId
