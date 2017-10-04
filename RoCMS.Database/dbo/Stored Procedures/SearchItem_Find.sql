CREATE PROCEDURE [dbo].[SearchItem_Find]
	@FulltextSearchQuery nvarchar(2000),
	@Entities String_Table readonly
AS
	IF (@FulltextSearchQuery IS NOT NULL AND @FulltextSearchQuery != '')
		SELECT s.SearchItemKey, s.[EntityName], s.[EntityId], s.[Text], s.Title, s.[Url], s.[Weight], s.[ImageId], s.[HeartId], tbl.[RANK] as [Rank]
			FROM [SearchItem] s JOIN FREETEXTTABLE([SearchItem], [SearchContent], @FulltextSearchQuery) tbl
			ON s.[FulltextKey]=tbl.[KEY]
			WHERE
				(NOT EXISTS (SELECT * FROM @Entities) OR (s.EntityName IN (SELECT Val FROM @Entities)))
			ORDER BY [Rank] DESC
	ELSE
		SELECT s.SearchItemKey, s.[EntityName], s.[EntityId], s.[Text], s.Title, s.[Url], s.[Weight], s.[ImageId], s.[HeartId], 0 as [Rank]
			FROM [SearchItem] s
			WHERE
				(NOT EXISTS (SELECT * FROM @Entities) OR (s.EntityName IN (SELECT Val FROM @Entities)))