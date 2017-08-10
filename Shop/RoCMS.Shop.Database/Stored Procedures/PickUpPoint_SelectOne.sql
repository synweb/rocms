CREATE PROCEDURE [Shop].[PickUpPoint_SelectOne]
@PickUpPointId int
AS
	SELECT * FROM [Shop].[PickUpPoint]
	WHERE [PickUpPointid]=@PickUpPointId
