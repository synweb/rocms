CREATE PROCEDURE [Shop].[Manufacturer_Delete]
@HeartId int
AS
	DELETE FROM [Shop].[Manufacturer]
	WHERE [HeartId]=@HeartId
