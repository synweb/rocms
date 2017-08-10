CREATE PROCEDURE [News].[NewsItemTag_Insert]
@NewsItemId int,
@TagId int
AS
	INSERT INTO [News].[NewsItemTag] ([NewsItemId], [TagId])
	VALUES (@NewsItemId, @TagId)
