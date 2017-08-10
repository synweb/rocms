CREATE TABLE [Shop].[PickUpPoint]
(
	[PickUpPointId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] NVARCHAR(250) NOT NULL, 
	[Partner] NVARCHAR(200) NULL, 
    [City] NVARCHAR(100) NOT NULL, 
    [Address] NVARCHAR(500) NOT NULL, 
	[Phone] NVARCHAR(200) NULL, 
    [Metro] NVARCHAR(100) NULL, 
    [Schedule] NVARCHAR(200) NULL, 
    [PaymentType] NVARCHAR(200) NULL, 
	[HowToReach] NVARCHAR(1000) NULL, 
    [Description] NVARCHAR(1000) NULL, 
    [ImageId] VARCHAR(30) NULL,

	CONSTRAINT [FK_PickUpPoint_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON DELETE SET NULL
)
