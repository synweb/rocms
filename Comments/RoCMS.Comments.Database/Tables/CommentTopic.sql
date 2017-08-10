CREATE TABLE [Comments].[CommentTopic]
(
	[CommentTopicId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[TargetType] VARCHAR(50) NULL,
	[TargetId] INT NULL,
	[TargetUrl] NVARCHAR(2000) NULL,
	[TargetTitle] NVARCHAR(300) NULL

)
