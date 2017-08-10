CREATE PROCEDURE [Shop].[RegularCustomerDiscount_SelectOne]
@DiscountId int
AS
	SELECT * FROM [Shop].[RegularCustomerDiscount]
		WHERE [DiscountId]=@DiscountId