CREATE TABLE [dbo].[OrderFormField]
(
	[OrderFormFieldId] INT NOT NULL PRIMARY KEY IDENTITY,
	[LabelText] nvarchar(100) NOT NULL,
	[ValueType] varchar(20) NOT NULL,
	[Required] bit NOT NULL DEFAULT 1,
	[OrderFormId] int NOT NULL, 
    
	[SortOrder] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_OrderFormField_ToOrderForm] FOREIGN KEY ([OrderFormId]) REFERENCES [OrderForm]([OrderFormId]) ON DELETE CASCADE
)
