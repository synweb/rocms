﻿--CREATE PROCEDURE [Shop].[Category_ExistsByRelativeUrl]
--	@RelativeUrl nvarchar(300)
--AS
--	IF EXISTS( SELECT * FROM [Shop].[Category] WHERE [RelativeUrl]=@RelativeUrl )
--		SELECT 1
--	ELSE
--		SELECT 0