CREATE PROCEDURE [Shop].[Cart_Insert]
@CartId uniqueidentifier,
@TotalDiscount decimal
AS
	INSERT INTO [Shop].[Cart] ([CartId], [TotalDiscount])
	VALUES (@CartId, @TotalDiscount)
