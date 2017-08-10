CREATE PROCEDURE [News].[Tag_SelectByPattern]
	@Pattern nvarchar(max),
	@Records int
AS
SELECT * FROM 
(SELECT TOP(@Records) [Tag].[Name] from
    (SELECT Count([NewsItemId]) as Mentions, [TagId] FROM [News].[NewsItemTag]
    GROUP BY  [TagId]) as tags
    JOIN [News].[Tag] on ([Tag].[TagId] = tags.[TagId]) and ([Name] Like @Pattern + '%')
    ORDER BY tags.Mentions desc
	) as TagsRes
ORDER BY TagsRes.[Name]

