CREATE PROCEDURE [Shop].[CartItem_Insert]
@CartId uniqueidentifier,
@GoodsId int,
@PackId int,
@Quantity int
AS
	INSERT INTO [Shop].[CartItem] ([CartId], [GoodsId], [PackId], [Quantity])
	VALUES (@CartId, @GoodsId, @PackId, @Quantity)
	SELECT @@IDENTITY
