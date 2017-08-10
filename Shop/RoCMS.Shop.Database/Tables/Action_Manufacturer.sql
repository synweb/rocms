CREATE TABLE [Shop].[Action_Manufacturer]
(
	[ActionId] INT NOT NULL , 
    [ManufacturerId] INT NOT NULL,
    PRIMARY KEY ([ManufacturerId], [ActionId]),
	CONSTRAINT [FK_ActionManufacturer_Action] FOREIGN KEY ([ActionId]) REFERENCES [Shop].[Action] ([ActionId]) ON DELETE CASCADE,
    CONSTRAINT [FK_ActionManufacturer_Manufacturer] FOREIGN KEY ([ManufacturerId]) REFERENCES [Shop].[Manufacturer] ([ManufacturerId]) ON DELETE CASCADE
)
