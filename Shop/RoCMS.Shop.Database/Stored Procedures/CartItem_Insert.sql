CREATE PROCEDURE [Shop].[CartItem_Insert]
@CartId uniqueidentifier,
@HeartId int,
@PackId int,
@Quantity int
AS
	INSERT INTO [Shop].[CartItem] ([CartId], [HeartId], [PackId], [Quantity])
	VALUES (@CartId, @HeartId, @PackId, @Quantity)
	SELECT @@IDENTITY
