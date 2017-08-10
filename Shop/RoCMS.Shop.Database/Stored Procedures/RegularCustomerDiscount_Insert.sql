CREATE PROCEDURE [Shop].[RegularCustomerDiscount_Insert]
@MinimalSum decimal,
@Discount int
AS
	INSERT INTO [Shop].[RegularCustomerDiscount] ([MinimalSum], [Discount])
	VALUES (@MinimalSum, @Discount)
	SELECT @@IDENTITY
