CREATE PROCEDURE [Shop].[RegularCustomerDiscount_Delete]
@DiscountId int
AS
	DELETE FROM [Shop].[RegularCustomerDiscount]
	WHERE [DiscountId]=@DiscountId
