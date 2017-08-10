CREATE PROCEDURE [Shop].[GoodsItem_SelectOneByRelativeUrl]
	@RelativeUrl nvarchar(300)
AS
	SELECT * FROM GoodsItem WHERE [RelativeUrl]=@RelativeUrl AND Deleted=0
