CREATE PROCEDURE [dbo].[SearchItem_FindStrict]
	@SearchQuery nvarchar(2000),
	@Entities String_Table readonly
AS
	SELECT s.SearchItemKey, s.[EntityName], s.[EntityId], s.[Text], s.Title, s.[Url], s.[Weight], s.[ImageId], s.[HeartId], 0 as [Rank]
		FROM [SearchItem] s 
		WHERE
			(NOT EXISTS (SELECT * FROM @Entities) OR (s.EntityName IN (SELECT Val FROM @Entities)))
			AND s.StrictSearch=1
			AND s.SearchContent LIKE '%'+@SearchQuery+'%'
		ORDER BY [Rank] DESC
		