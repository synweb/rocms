CREATE PROCEDURE [Shop].[RegularCustomerDiscount_Update]
@MinimalSum decimal,
@Discount int,
@DiscountId int
AS
	UPDATE [Shop].[RegularCustomerDiscount] SET
		[MinimalSum]=@MinimalSum,
		[Discount]=@Discount
	WHERE [DiscountId]=@DiscountId
