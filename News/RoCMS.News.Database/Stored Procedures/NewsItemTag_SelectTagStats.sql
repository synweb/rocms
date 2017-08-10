CREATE PROCEDURE [News].[NewsItemTag_SelectTagStats]
@Records int
AS
SELECT * FROM 
(SELECT TOP(@Records) tags.Mentions, tags.[TagId], [Tag].[Name] from
    (SELECT Count([NewsItemId]) as Mentions, [TagId] FROM [News].[NewsItemTag]
    GROUP BY  [TagId]) as tags
    JOIN [News].[Tag] on [Tag].[TagId] = tags.[TagId]
    ORDER BY tags.Mentions desc
	) as TagsRes
ORDER BY TagsRes.[Name]
