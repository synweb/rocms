CREATE TABLE [dbo].[UserCmsResource] (
    [CmsResourceId] INT NOT NULL,
    [UserId]        INT NOT NULL,
    CONSTRAINT [PK_UserCmsResource] PRIMARY KEY CLUSTERED ([UserId] ASC, [CmsResourceId] ASC),
    CONSTRAINT [FK_UserCmsResource_ToCmsResource] FOREIGN KEY ([CmsResourceId]) REFERENCES [dbo].[CmsResource] ([CmsResourceId]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserCmsResource_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE
);

