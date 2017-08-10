CREATE PROCEDURE [Shop].[GoodsItem_ExistsByRelativeUrl]
	@RelativeUrl nvarchar(300)
AS
	IF EXISTS( SELECT * FROM [Shop].[GoodsItem] 
	WHERE [RelativeUrl]=@RelativeUrl AND [Deleted]=0)
		SELECT 1
	ELSE
		SELECT 0