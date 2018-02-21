CREATE PROCEDURE [Shop].[FavouriteItem_Delete]
	@SessionId uniqueidentifier,
	@HeartId int
AS
	DELETE FROM [shop].FavouriteItem WHERE SessionId = @SessionId AND HeartId = @HeartId
