CREATE TABLE [Shop].[RegularCustomerDiscount]
(
	[DiscountId] INT NOT NULL IDENTITY(1,1),
    [CreationDate]  DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[MinimalSum] DECIMAL (18,2) NOT NULL,
	[Discount] INT NOT NULL, 
    CONSTRAINT [PK_RegularCustomerDiscount] PRIMARY KEY ([DiscountId]), 
    CONSTRAINT [AK_RegularCustomerDiscount_MinSum] UNIQUE ([MinimalSum])
)
