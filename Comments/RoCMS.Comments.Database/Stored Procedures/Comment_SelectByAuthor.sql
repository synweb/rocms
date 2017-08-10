CREATE PROCEDURE [Comments].[Comment_SelectByAuthor]
	@AuthorId int,
	@Moderated BIT = NULL
AS
		IF @Moderated IS NULL
		SELECT * FROM [Comments].[Comment] WHERE [AuthorId]=@AuthorId
			ORDER BY [CreationDate] DESC
	ELSE
		SELECT * FROM [Comments].[Comment] WHERE [AuthorId]=@AuthorId AND [Moderated]=@Moderated
			ORDER BY [CreationDate] DESC
