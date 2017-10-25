CREATE PROCEDURE [Comments].[Comment_SelectByHeart]
	@HeartId int,
	@Moderated BIT = NULL
AS
	IF @Moderated IS NULL
		SELECT * FROM [Comments].[Comment] WHERE [HeartId]=@HeartId
			ORDER BY [CreationDate] DESC
	ELSE
		SELECT * FROM [Comments].[Comment] WHERE [HeartId]=@HeartId AND [Moderated]=@Moderated
			ORDER BY [CreationDate] DESC