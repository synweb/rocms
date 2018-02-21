CREATE PROCEDURE [Shop].[FavouriteItem_Select]
	@SessionId uniqueidentifier
AS
	SELECT * FROM [shop].FavouriteItem
	WHERE SessionId = @SessionId
	ORDER BY CreationDate DESC
