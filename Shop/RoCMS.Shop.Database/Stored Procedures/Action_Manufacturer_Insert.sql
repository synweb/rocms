CREATE PROCEDURE [Shop].[Action_Manufacturer_Insert]
@ActionId int,
@ManufacturerId int
AS
	INSERT INTO [Shop].[Action_Manufacturer] ([ActionId], [ManufacturerId])
	VALUES (@ActionId, @ManufacturerId)
