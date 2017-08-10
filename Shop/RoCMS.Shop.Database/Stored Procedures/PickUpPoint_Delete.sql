CREATE PROCEDURE [Shop].[PickUpPoint_Delete]
@PickUpPointId int
AS
	DELETE FROM [Shop].[PickUpPoint]
	WHERE [PickUpPointId]=@PickUpPointId
