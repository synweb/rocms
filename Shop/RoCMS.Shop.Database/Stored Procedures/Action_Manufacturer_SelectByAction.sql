CREATE PROCEDURE [Shop].[Action_Manufacturer_SelectByAction]
@ActionId int
AS
	SELECT * FROM [Shop].[Action_Manufacturer]
		WHERE [ActionId]=@ActionId
