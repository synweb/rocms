CREATE PROCEDURE [Shop].[Action_Manufacturer_Delete]
@ActionId int,
@ManufacturerId int
AS
	DELETE FROM [Shop].[Action_Manufacturer]
	WHERE [ActionId]=@ActionId
		 AND [ManufacturerId]=@ManufacturerId
