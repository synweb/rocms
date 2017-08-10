CREATE PROCEDURE [Shop].[Cart_Update]
	@CartId uniqueidentifier,
	@TotalDiscount decimal
AS
	UPDATE [Shop].[Cart]
		SET [TotalDiscount]=@TotalDiscount
		WHERE [CartId]=@CartId