CREATE TABLE [News].[Blog_User]
(
	[BlogId] INT NOT NULL, 
    [UserId] INT NOT NULL, 
	
	CONSTRAINT [FK_Blog_User_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([UserId]) ON DELETE CASCADE,
	CONSTRAINT [FK_Blog_User_Blog] FOREIGN KEY ([BlogId]) REFERENCES [News].[Blog]([BlogId]) ON DELETE CASCADE, 
    CONSTRAINT [PK_Blog_User] PRIMARY KEY ([BlogId], [UserId])
)