CREATE PROCEDURE [Shop].[FavouriteItem_Insert]
	@SessionId uniqueidentifier,
	@HeartId int
AS
	INSERT INTO [shop].FavouriteItem (SessionId, HeartId) VALUES (@SessionId, @HeartId)
