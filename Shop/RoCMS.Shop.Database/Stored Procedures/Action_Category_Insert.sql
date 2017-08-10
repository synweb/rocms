CREATE PROCEDURE [Shop].[Action_Category_Insert]
@ActionId int,
@CategoryId int
AS
	INSERT INTO [Shop].[Action_Category] ([ActionId], [CategoryId])
	VALUES (@ActionId, @CategoryId)
