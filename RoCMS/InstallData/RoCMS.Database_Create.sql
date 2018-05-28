

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;

GO
USE [master];

GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION OFF;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET AUTO_UPDATE_STATISTICS_ASYNC OFF,
                PAGE_VERIFY NONE,
                DATE_CORRELATION_OPTIMIZATION OFF,
                DISABLE_BROKER,
                PARAMETERIZATION SIMPLE,
                SUPPLEMENTAL_LOGGING OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET TRUSTWORTHY OFF,
        DB_CHAINING OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
IF IS_SRVROLEMEMBER(N'sysadmin') = 1
    BEGIN
        IF EXISTS (SELECT 1
                   FROM   [master].[dbo].[sysdatabases]
                   WHERE  [name] = N'$(DatabaseName)')
            BEGIN
                EXECUTE sp_executesql N'ALTER DATABASE [$(DatabaseName)]
    SET HONOR_BROKER_PRIORITY OFF 
    WITH ROLLBACK IMMEDIATE';
            END
    END
ELSE
    BEGIN
        PRINT N'The database settings cannot be modified. You must be a SysAdmin to apply these settings.';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET FILESTREAM(NON_TRANSACTED_ACCESS = OFF),
                CONTAINMENT = NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
USE [$(DatabaseName)];


GO
IF fulltextserviceproperty(N'IsFulltextInstalled') = 1
    EXECUTE sp_fulltext_database 'enable';


GO
PRINT N'Creating [News]...';


GO
CREATE SCHEMA [News]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating [Comments]...';


GO
CREATE SCHEMA [Comments]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating [Search_FullTextCatalog]...';


GO
CREATE FULLTEXT CATALOG [Search_FullTextCatalog]
    AUTHORIZATION [dbo];


GO
PRINT N'Creating [dbo].[Int_String_Table]...';


GO
CREATE TYPE [dbo].[Int_String_Table] AS TABLE (
    [Key] INT           NULL,
    [Val] VARCHAR (255) NULL);


GO
PRINT N'Creating [dbo].[Int_Table]...';


GO
CREATE TYPE [dbo].[Int_Table] AS TABLE (
    [Val] INT NULL);


GO
PRINT N'Creating [dbo].[String_Table]...';


GO
CREATE TYPE [dbo].[String_Table] AS TABLE (
    [Val] VARCHAR (255) NULL);


GO
PRINT N'Creating [dbo].[Album]...';


GO
CREATE TABLE [dbo].[Album] (
    [AlbumId]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50)  NOT NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [WatermarkImageId]      VARCHAR(30)   NULL,
    [CreationDate] DATETIME       NOT NULL,
    [OwnerId]      INT            NULL,
    CONSTRAINT [PK_AlbumSet] PRIMARY KEY CLUSTERED ([AlbumId] ASC)
);


GO
PRINT N'Creating [dbo].[Block]...';


GO
CREATE TABLE [dbo].[Block] (
    [BlockId] INT            IDENTITY (1, 1) NOT NULL,
    [Title]   NVARCHAR (MAX) NOT NULL,
    [Content] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Block] PRIMARY KEY CLUSTERED ([BlockId] ASC)
);


GO
PRINT N'Creating [dbo].[CmsResource]...';


GO
CREATE TABLE [dbo].[CmsResource] (
    [CmsResourceId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          VARCHAR (50)   NOT NULL,
    [Description]   NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([CmsResourceId] ASC),
    UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
PRINT N'Creating [dbo].[Country]...';


GO
CREATE TABLE [dbo].[Country] (
    [CountryId] INT           NOT NULL,
    [Name]      NVARCHAR (70) NOT NULL,
    PRIMARY KEY CLUSTERED ([CountryId] ASC)
);


GO
PRINT N'Creating [dbo].[FormRequest]...';


GO
CREATE TABLE [dbo].[FormRequest] (
    [FormRequestId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (50)  NULL,
    [Email]         NVARCHAR (50)  NULL,
    [Phone]         NVARCHAR (50)  NULL,
    [Text]          NVARCHAR (MAX) NULL,
    [CreationDate]  DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([FormRequestId] ASC)
);


GO
PRINT N'Creating [dbo].[Image]...';


GO
CREATE TABLE [dbo].[Image] (
    [ImageId]         VARCHAR (30)   NOT NULL,
    [CreationDate]    DATETIME       NOT NULL,
    [InitialFilename] NVARCHAR (257) NULL,
    CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED ([ImageId] ASC)
);


GO
PRINT N'Creating [dbo].[ImageInAlbum]...';


GO
CREATE TABLE [dbo].[ImageInAlbum] (
    [AlbumId]        INT            NOT NULL,
    [ImageId]        VARCHAR (30)   NOT NULL,
    [Title]          NVARCHAR (MAX) NULL,
    [Description]    NVARCHAR (MAX) NULL,
    [SortOrder]      INT            NOT NULL,
    [DestinationUrl] NVARCHAR (250) NULL,
    CONSTRAINT [PK_ImageInAlbum] PRIMARY KEY CLUSTERED ([AlbumId] ASC, [ImageId] ASC)
);


GO
PRINT N'Creating [dbo].[InterfaceString]...';


GO
CREATE TABLE [dbo].[InterfaceString] (
    [Key]   VARCHAR (200)  NOT NULL,
    [Value] NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Key] ASC)
);


GO
PRINT N'Creating [dbo].[OrderForm]...';


GO
CREATE TABLE [dbo].[OrderForm] (
    [OrderFormId]             INT            IDENTITY (1, 1) NOT NULL,
    [EmailSubject]            NVARCHAR (300) NULL,
    [DateInEmailSubject]      BIT            NOT NULL,
    [Email]                   NVARCHAR (100) NULL,
    [BccEmail]                NVARCHAR (100) NULL,
    [HtmlTemplate]            NVARCHAR (MAX) NULL,
    [RedirectUrl]             NVARCHAR (500) NULL,
    [SuccessMessage]          NVARCHAR (500) NULL,
    [MetricsCode]             NVARCHAR (50)  NULL,
    [FileAttachmentEnabled]   BIT            NOT NULL,
    [MaxFileAttachmentsCount] INT            NOT NULL,
    [Title]                   NVARCHAR (200) NOT NULL,
    [EmailTemplate]           NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([OrderFormId] ASC)
);


GO
PRINT N'Creating [dbo].[OrderFormField]...';


GO
CREATE TABLE [dbo].[OrderFormField] (
    [OrderFormFieldId] INT            IDENTITY (1, 1) NOT NULL,
    [LabelText]        NVARCHAR (100) NOT NULL,
    [ValueType]        VARCHAR (20)   NOT NULL,
    [Required]         BIT            NOT NULL,
    [OrderFormId]      INT            NOT NULL,
    [SortOrder]        INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderFormFieldId] ASC)
);

GO
PRINT N'Creating [dbo].[Mail]...';


GO
CREATE TABLE [dbo].[Mail] (
    [MailId]       INT           IDENTITY (1, 1) NOT NULL,
    [CreationDate] DATETIME      NOT NULL,
    [Body]         NVARCHAR (MAX) NOT NULL,
    [Subject]      NVARCHAR (MAX) NULL,
    [Receiver]     NVARCHAR (MAX) NOT NULL,
    [Sent]         BIT           NOT NULL,
    [ErrorMessage] NVARCHAR (MAX) NULL,
    [Attaches]     NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([MailId] ASC)
);


GO
PRINT N'Creating [dbo].[Menu]...';


GO
CREATE TABLE [dbo].[Menu] (
    [MenuId] INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (200) NOT NULL,
    CONSTRAINT [PK_Menu] PRIMARY KEY CLUSTERED ([MenuId] ASC)
);


GO
PRINT N'Creating [dbo].[MenuItem]...';


GO
CREATE TABLE [dbo].[MenuItem] (
    [MenuItemId]       INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (200) NOT NULL,
    [MenuId]           INT            NOT NULL,
    [ParentMenuItemId] INT            NULL,
    [PageUrl]          NVARCHAR (300) NULL,
    [SortOrder]        INT            NOT NULL,
    [BlockId]          INT            NULL,
    CONSTRAINT [PK_MenuItem] PRIMARY KEY CLUSTERED ([MenuItemId] ASC)
);


GO
PRINT N'Creating [dbo].[Page]...';


GO
CREATE TABLE [dbo].[Page] (
    [Title]         NVARCHAR (MAX) NOT NULL,
    [Annotation]    NVARCHAR (MAX) NOT NULL,
    [Content]       NVARCHAR (MAX) NOT NULL,
    [CreationDate]  DATETIME       NOT NULL,
    [RelativeUrl]   NVARCHAR (300) NOT NULL,
    [Keywords]      NVARCHAR (MAX) NULL,
    [PageId]        INT            IDENTITY (1, 1) NOT NULL,
    [ParentPageId]  INT            NULL,
    [HideInSitemap] BIT            NOT NULL,
    [Header]        NVARCHAR (MAX) NULL,
    [Styles]        NVARCHAR (MAX) NULL,
    [Scripts]       NVARCHAR (MAX) NULL,
    [Layout]        VARCHAR (300)  NOT NULL,
	[AdditionalHeaders] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED ([PageId] ASC),
    UNIQUE NONCLUSTERED ([RelativeUrl] ASC)
);


GO
PRINT N'Creating [dbo].[PasswordTicket]...';


GO
CREATE TABLE [dbo].[PasswordTicket] (
    [TicketId]       INT              IDENTITY (1, 1) NOT NULL,
    [UserId]         INT              NOT NULL,
    [Token]          UNIQUEIDENTIFIER NOT NULL,
    [Used]           BIT              NOT NULL,
    [CreationDate]   DATETIME         NOT NULL,
    [ExpirationDate] DATETIME         NOT NULL,
    [UseDate]        DATETIME         NULL,
    CONSTRAINT [PK_PasswordTicket] PRIMARY KEY CLUSTERED ([TicketId] ASC),
    CONSTRAINT [AK_PasswordTicket_Token] UNIQUE NONCLUSTERED ([Token] ASC)
);


GO
PRINT N'Creating [dbo].[Review]...';


GO
CREATE TABLE [dbo].[Review] (
    [ReviewId]     INT            IDENTITY (1, 1) NOT NULL,
    [Author]       NVARCHAR (50)  NOT NULL,
    [City]         NVARCHAR (50)  NULL,
    [Email]        NVARCHAR (100) NULL,
    [Text]         NVARCHAR (MAX) NOT NULL,
    [Moderated]    BIT            NOT NULL,
    [VK]           NVARCHAR (50)  NULL,
    [CreationDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED ([ReviewId] DESC)
);


GO
PRINT N'Creating [dbo].[SearchItem]...';


GO
CREATE TABLE [dbo].[SearchItem] (
    [SearchItemKey] VARCHAR (50)   NOT NULL,
    [EntityName]    VARCHAR (200)  NOT NULL,
    [EntityId]      NVARCHAR (100) NOT NULL,
    [FulltextKey]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [SearchContent] NVARCHAR (MAX) NOT NULL,
    [Text]          NVARCHAR (500) NOT NULL,
    [Title]         NVARCHAR (200) NOT NULL,
    [Url]           NVARCHAR (500) NOT NULL,
    [ImageId]       VARCHAR (30)   NULL,
    [Weight]        INT            NOT NULL,
    [CreationDate]  DATETIME       NOT NULL,
    [UpdateDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_SearchItem] PRIMARY KEY CLUSTERED ([SearchItemKey] ASC, [EntityName] ASC, [EntityId] ASC),
    CONSTRAINT [AK_SearchItem_Fulltext] UNIQUE NONCLUSTERED ([FulltextKey] ASC)
);


GO
PRINT N'Creating [dbo].[Setting]...';


GO
CREATE TABLE [dbo].[Setting] (
    [SettingId] INT            IDENTITY (1, 1) NOT NULL,
    [Key]       NVARCHAR (100) NOT NULL,
    [Value]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([SettingId] ASC),
    CONSTRAINT [AK_Setting] UNIQUE NONCLUSTERED ([Key] ASC)
);


GO
PRINT N'Creating [dbo].[Slide]...';


GO
CREATE TABLE [dbo].[Slide] (
    [SlideId]     INT             IDENTITY (1, 1) NOT NULL,
    [SliderId]    INT             NOT NULL,
    [Title]       NVARCHAR (50)   NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    [ImageId]     VARCHAR (30)    NOT NULL,
    [Link]        NVARCHAR (MAX)  NULL,
    [SortOrder]   INT             NOT NULL,
    CONSTRAINT [PK_Slide] PRIMARY KEY NONCLUSTERED ([SlideId] ASC)
);


GO
PRINT N'Creating [dbo].[Slide].[IX_Slide_Sort]...';


GO
CREATE CLUSTERED INDEX [IX_Slide_Sort]
    ON [dbo].[Slide]([SortOrder] ASC);


GO
PRINT N'Creating [dbo].[Slider]...';


GO
CREATE TABLE [dbo].[Slider] (
    [SliderId] INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Slider] PRIMARY KEY CLUSTERED ([SliderId] ASC)
);


GO
PRINT N'Creating [dbo].[User]...';


GO
CREATE TABLE [dbo].[User] (
    [UserId]         INT             IDENTITY (1, 1) NOT NULL,
    [CreationDate]   DATETIME        NOT NULL,
    [Username]       NVARCHAR (50)   NOT NULL,
    [Password]       NVARCHAR (200)   NOT NULL,
    [Email]          NVARCHAR (100)  NULL,
    [EmailConfirmed] BIT             NOT NULL,
    [Description]    NVARCHAR (1000) NULL,
    [Vk]             NVARCHAR (100)  NULL,
    [Fb]             NVARCHAR (100)  NULL,
    [GoogleP]        NVARCHAR (100)  NULL,
    [Twitter]        NVARCHAR (100)  NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [AK_User] UNIQUE NONCLUSTERED ([Username] ASC)
);


GO
PRINT N'Creating [dbo].[UserCmsResource]...';


GO
CREATE TABLE [dbo].[UserCmsResource] (
    [CmsResourceId] INT NOT NULL,
    [UserId]        INT NOT NULL,
    CONSTRAINT [PK_UserCmsResource] PRIMARY KEY CLUSTERED ([UserId] ASC, [CmsResourceId] ASC)
);


GO
PRINT N'Creating [dbo].[Video]...';


GO
CREATE TABLE [dbo].[Video] (
    [VideoId]      VARCHAR (50)   NOT NULL,
    [AlbumId]      INT            NOT NULL,
    [ImageId]      VARCHAR (30)   NULL,
    [CreationDate] DATETIME       NOT NULL,
    [Title]        NVARCHAR (MAX) NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [SortOrder]    INT            NOT NULL,
    CONSTRAINT [PK_Video] PRIMARY KEY NONCLUSTERED ([VideoId] ASC)
);


GO
PRINT N'Creating [dbo].[Video].[IX_Video_CreationDate]...';


GO
CREATE NONCLUSTERED INDEX [IX_Video_CreationDate]
    ON [dbo].[Video]([CreationDate] DESC);


GO
PRINT N'Creating [dbo].[VideoAlbum]...';


GO
CREATE TABLE [dbo].[VideoAlbum] (
    [AlbumId]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NOT NULL,
    [CreationDate] DATETIME      NOT NULL,
    [OwnerId]      INT           NULL,
    CONSTRAINT [PK_VideoAlbum] PRIMARY KEY CLUSTERED ([AlbumId] ASC)
);


GO
PRINT N'Creating [News].[NewsItemTag]...';


GO
CREATE TABLE [News].[NewsItemTag] (
    [NewsItemId] INT NOT NULL,
    [TagId]      INT NOT NULL,
    CONSTRAINT [PK_NewsTag] PRIMARY KEY CLUSTERED ([NewsItemId] ASC, [TagId] ASC)
);


GO
PRINT N'Creating [News].[Tag]...';


GO
CREATE TABLE [News].[Tag] (
    [TagId]        INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate] DATETIME       NOT NULL,
    [Name]         NVARCHAR (200) NOT NULL,
    PRIMARY KEY CLUSTERED ([TagId] ASC),
    CONSTRAINT [AK_Tag_Name] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
PRINT N'Creating [News].[NewsItem_Category]...';


GO
CREATE TABLE [News].[NewsItem_Category] (
    [NewsItemId] INT NOT NULL,
    [CategoryId] INT NOT NULL,
    CONSTRAINT [PK_NewsItem_Category] PRIMARY KEY CLUSTERED ([NewsItemId] ASC, [CategoryId] ASC)
);


GO
PRINT N'Creating [News].[Category]...';


GO
CREATE TABLE [News].[Category] (
    [CategoryId]       INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate]     DATETIME       NOT NULL,
    [Name]             NVARCHAR (MAX) NOT NULL,
    [ParentCategoryId] INT            NULL,
    [SortOrder]        INT            NOT NULL,
    [Hidden]           BIT            NOT NULL,
    [RelativeUrl]      NVARCHAR (300) NOT NULL,
    PRIMARY KEY CLUSTERED ([CategoryId] ASC),
    UNIQUE NONCLUSTERED ([RelativeUrl] ASC)
);


GO
PRINT N'Creating [News].[NewsItem]...';


GO
CREATE TABLE [News].[NewsItem] (
    [NewsId]            INT            IDENTITY (1, 1) NOT NULL,
    [Title]             NVARCHAR (MAX) NOT NULL,
    [Text]              NVARCHAR (MAX) NOT NULL,
    [PostingDate]       DATETIME       NOT NULL,
    [Description]       NVARCHAR (MAX) NOT NULL,
    [MetaDescription]   NVARCHAR (MAX) NULL,
    [Keywords]          NVARCHAR (MAX) NULL,
    [CreationDate]      DATETIME       NOT NULL,
    [AuthorId]          INT            NOT NULL,
    [ImageId]           VARCHAR (30)   NULL,
    [RelativeUrl]       NVARCHAR (300) NOT NULL,
    [CommentTopicId]    INT            NULL,
    [RecordType]        VARCHAR (20)   NOT NULL,
    [Filename]          NVARCHAR (200) NULL,
    [VideoId]           VARCHAR (50)   NULL,
    [BlogId]            INT            NULL,
    [RelatedNewsItemId] INT            NULL,
    [EventDate]         DATETIME       NULL,
	[AdditionalHeaders] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_NewsSet] PRIMARY KEY CLUSTERED ([NewsId] ASC),
    UNIQUE NONCLUSTERED ([RelativeUrl] ASC)
);


GO
PRINT N'Creating [News].[NewsItem].[IX_FK_NewsUser]...';


GO
CREATE NONCLUSTERED INDEX [IX_FK_NewsUser]
    ON [News].[NewsItem]([AuthorId] ASC);


GO
PRINT N'Creating [News].[Blog_User]...';


GO
CREATE TABLE [News].[Blog_User] (
    [BlogId] INT NOT NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT [PK_Blog_User] PRIMARY KEY CLUSTERED ([BlogId] ASC, [UserId] ASC)
);


GO
PRINT N'Creating [News].[Blog]...';


GO
CREATE TABLE [News].[Blog] (
    [BlogId]       INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate] DATETIME       NOT NULL,
    [Title]        NVARCHAR (500) NULL,
    [Subtitle]     NVARCHAR (500) NULL,
    [OwnerId]      INT            NULL,
    [RelativeUrl]  NVARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([BlogId] ASC)
);


GO
PRINT N'Creating [News].[Blog].[IX_Blog_Url]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Blog_Url]
    ON [News].[Blog]([RelativeUrl] ASC) WHERE ([RelativeUrl] IS NOT NULL);


GO
PRINT N'Creating [Comments].[Comment]...';


GO
CREATE TABLE [Comments].[Comment] (
    [CommentId]       INT            IDENTITY (1, 1) NOT NULL,
    [ParentCommentId] INT            NULL,
    [CommentTopicId]  INT            NOT NULL,
    [Text]            NVARCHAR (MAX) NOT NULL,
    [Moderated]       BIT            NOT NULL,
    [AuthorId]        INT            NULL,
    [CreationDate]    DATETIME       NOT NULL,
    [Deleted]         BIT            NOT NULL,
    [Url]             NVARCHAR (200) NULL,
    [Email]           NVARCHAR (200) NULL,
    [Name]            NVARCHAR (200) NULL,
    CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED ([CommentId] DESC)
);


GO
PRINT N'Creating [Comments].[Comment].[IX_Comment_Author]...';


GO
CREATE NONCLUSTERED INDEX [IX_Comment_Author]
    ON [Comments].[Comment]([AuthorId] ASC);


GO
PRINT N'Creating [Comments].[Comment].[IX_Comment_Date]...';


GO
CREATE NONCLUSTERED INDEX [IX_Comment_Date]
    ON [Comments].[Comment]([CreationDate] DESC);


GO
PRINT N'Creating [Comments].[CommentTopic]...';


GO
CREATE TABLE [Comments].[CommentTopic] (
    [CommentTopicId] INT             IDENTITY (1, 1) NOT NULL,
    [TargetType]     VARCHAR (50)    NULL,
    [TargetId]       INT             NULL,
    [TargetUrl]      NVARCHAR (2000) NULL,
    [TargetTitle]    NVARCHAR (300)  NULL,
    PRIMARY KEY CLUSTERED ([CommentTopicId] ASC)
);


GO
PRINT N'Creating unnamed constraint on [dbo].[Album]...';


GO
ALTER TABLE [dbo].[Album]
    ADD DEFAULT (GETUTCDATE()) FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[CmsResource]...';


GO
ALTER TABLE [dbo].[CmsResource]
    ADD DEFAULT '' FOR [Description];


GO
PRINT N'Creating unnamed constraint on [dbo].[FormRequest]...';


GO
ALTER TABLE [dbo].[FormRequest]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[OrderForm]...';


GO
ALTER TABLE [dbo].[OrderForm]
    ADD DEFAULT 1 FOR [DateInEmailSubject];


GO
PRINT N'Creating unnamed constraint on [dbo].[OrderForm]...';


GO
ALTER TABLE [dbo].[OrderForm]
    ADD DEFAULT 0 FOR [FileAttachmentEnabled];


GO
PRINT N'Creating unnamed constraint on [dbo].[OrderForm]...';


GO
ALTER TABLE [dbo].[OrderForm]
    ADD DEFAULT 0 FOR [MaxFileAttachmentsCount];


GO
PRINT N'Creating unnamed constraint on [dbo].[OrderFormField]...';


GO
ALTER TABLE [dbo].[OrderFormField]
    ADD DEFAULT 1 FOR [Required];


GO
PRINT N'Creating unnamed constraint on [dbo].[OrderFormField]...';


GO
ALTER TABLE [dbo].[OrderFormField]
    ADD DEFAULT 0 FOR [SortOrder];

GO
PRINT N'Creating unnamed constraint on [dbo].[Image]...';


GO
ALTER TABLE [dbo].[Image]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[ImageInAlbum]...';


GO
ALTER TABLE [dbo].[ImageInAlbum]
    ADD DEFAULT 0 FOR [SortOrder];


GO
PRINT N'Creating unnamed constraint on [dbo].[Mail]...';


GO
ALTER TABLE [dbo].[Mail]
    ADD DEFAULT (GETUTCDATE()) FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[MenuItem]...';


GO
ALTER TABLE [dbo].[MenuItem]
    ADD DEFAULT ((0)) FOR [SortOrder];


GO
PRINT N'Creating unnamed constraint on [dbo].[Page]...';


GO
ALTER TABLE [dbo].[Page]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[Page]...';


GO
ALTER TABLE [dbo].[Page]
    ADD DEFAULT 0 FOR [HideInSitemap];


GO
PRINT N'Creating unnamed constraint on [dbo].[Page]...';


GO
ALTER TABLE [dbo].[Page]
    ADD DEFAULT 'clientLayout' FOR [Layout];


GO
PRINT N'Creating unnamed constraint on [dbo].[PasswordTicket]...';


GO
ALTER TABLE [dbo].[PasswordTicket]
    ADD DEFAULT 0 FOR [Used];


GO
PRINT N'Creating unnamed constraint on [dbo].[PasswordTicket]...';


GO
ALTER TABLE [dbo].[PasswordTicket]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[Review]...';


GO
ALTER TABLE [dbo].[Review]
    ADD DEFAULT 0 FOR [Moderated];


GO
PRINT N'Creating unnamed constraint on [dbo].[Review]...';


GO
ALTER TABLE [dbo].[Review]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[SearchItem]...';


GO
ALTER TABLE [dbo].[SearchItem]
    ADD DEFAULT '' FOR [EntityName];


GO
PRINT N'Creating unnamed constraint on [dbo].[SearchItem]...';


GO
ALTER TABLE [dbo].[SearchItem]
    ADD DEFAULT '' FOR [EntityId];


GO
PRINT N'Creating unnamed constraint on [dbo].[SearchItem]...';


GO
ALTER TABLE [dbo].[SearchItem]
    ADD DEFAULT 1 FOR [Weight];


GO
PRINT N'Creating unnamed constraint on [dbo].[SearchItem]...';


GO
ALTER TABLE [dbo].[SearchItem]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[SearchItem]...';


GO
ALTER TABLE [dbo].[SearchItem]
    ADD DEFAULT GETUTCDATE() FOR [UpdateDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[Slide]...';


GO
ALTER TABLE [dbo].[Slide]
    ADD DEFAULT 0 FOR [SortOrder];


GO
PRINT N'Creating unnamed constraint on [dbo].[User]...';


GO
ALTER TABLE [dbo].[User]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [dbo].[User]...';


GO
ALTER TABLE [dbo].[User]
    ADD DEFAULT 0 FOR [EmailConfirmed];


GO
PRINT N'Creating unnamed constraint on [dbo].[Video]...';


GO
ALTER TABLE [dbo].[Video]
    ADD DEFAULT ((0)) FOR [SortOrder];


GO
PRINT N'Creating unnamed constraint on [dbo].[VideoAlbum]...';


GO
ALTER TABLE [dbo].[VideoAlbum]
    ADD DEFAULT (GETUTCDATE()) FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [News].[Tag]...';


GO
ALTER TABLE [News].[Tag]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [News].[Category]...';


GO
ALTER TABLE [News].[Category]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [News].[Category]...';


GO
ALTER TABLE [News].[Category]
    ADD DEFAULT 0 FOR [SortOrder];


GO
PRINT N'Creating unnamed constraint on [News].[Category]...';


GO
ALTER TABLE [News].[Category]
    ADD DEFAULT 0 FOR [Hidden];


GO
PRINT N'Creating unnamed constraint on [News].[NewsItem]...';


GO
ALTER TABLE [News].[NewsItem]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [News].[NewsItem]...';


GO
ALTER TABLE [News].[NewsItem]
    ADD DEFAULT 'Default' FOR [RecordType];


GO
PRINT N'Creating unnamed constraint on [News].[Blog]...';


GO
ALTER TABLE [News].[Blog]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [Comments].[Comment]...';


GO
ALTER TABLE [Comments].[Comment]
    ADD DEFAULT GETUTCDATE() FOR [CreationDate];


GO
PRINT N'Creating unnamed constraint on [Comments].[Comment]...';


GO
ALTER TABLE [Comments].[Comment]
    ADD DEFAULT 0 FOR [Deleted];


GO
PRINT N'Creating Full-text Index on [dbo].[SearchItem]...';


GO
CREATE FULLTEXT INDEX ON [dbo].[SearchItem]
    ([SearchContent] LANGUAGE 1049)
    KEY INDEX [AK_SearchItem_Fulltext]
    ON [Search_FullTextCatalog];


GO
PRINT N'Creating [dbo].[FK_AlbumSet_User]...';


GO
ALTER TABLE [dbo].[Album]
    ADD CONSTRAINT [FK_AlbumSet_User] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User] ([UserId]);


GO
PRINT N'Creating [dbo].[FK_Album_WatermarkImageId]...';

ALTER TABLE [dbo].[Album]
    ADD CONSTRAINT [FK_Album_WatermarkImageId] FOREIGN KEY ([WatermarkImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL;


GO
PRINT N'Creating [dbo].[FK_ImageInAlbum_AlbumId]...';


GO
ALTER TABLE [dbo].[ImageInAlbum]
    ADD CONSTRAINT [FK_ImageInAlbum_AlbumId] FOREIGN KEY ([AlbumId]) REFERENCES [dbo].[Album] ([AlbumId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_ImageInAlbum_ImageId]...';


GO
ALTER TABLE [dbo].[ImageInAlbum]
    ADD CONSTRAINT [FK_ImageInAlbum_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_MenuItem_Block]...';


GO
ALTER TABLE [dbo].[MenuItem]
    ADD CONSTRAINT [FK_MenuItem_Block] FOREIGN KEY ([BlockId]) REFERENCES [dbo].[Block] ([BlockId]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_MenuItem_MenuId]...';


GO
ALTER TABLE [dbo].[MenuItem]
    ADD CONSTRAINT [FK_MenuItem_MenuId] FOREIGN KEY ([MenuId]) REFERENCES [dbo].[Menu] ([MenuId]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_MenuItem_PageUrl]...';


GO
ALTER TABLE [dbo].[MenuItem]
    ADD CONSTRAINT [FK_MenuItem_PageUrl] FOREIGN KEY ([PageUrl]) REFERENCES [dbo].[Page] ([RelativeUrl]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_MenuItem_ParentMenuItemId]...';


GO
ALTER TABLE [dbo].[MenuItem]
    ADD CONSTRAINT [FK_MenuItem_ParentMenuItemId] FOREIGN KEY ([ParentMenuItemId]) REFERENCES [dbo].[MenuItem] ([MenuItemId]);


GO
PRINT N'Creating [dbo].[FK_Page_ToPage]...';


GO
ALTER TABLE [dbo].[Page]
    ADD CONSTRAINT [FK_Page_ToPage] FOREIGN KEY ([ParentPageId]) REFERENCES [dbo].[Page] ([PageId]);


GO
PRINT N'Creating [dbo].[FK_PasswordTicket_User]...';


GO
ALTER TABLE [dbo].[PasswordTicket]
    ADD CONSTRAINT [FK_PasswordTicket_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_SearchItem_Image]...';


GO
ALTER TABLE [dbo].[SearchItem]
    ADD CONSTRAINT [FK_SearchItem_Image] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]);


GO
PRINT N'Creating [dbo].[FK_Slide_ImageId]...';


GO
ALTER TABLE [dbo].[Slide]
    ADD CONSTRAINT [FK_Slide_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON DELETE CASCADE ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Slide_SliderId]...';


GO
ALTER TABLE [dbo].[Slide]
    ADD CONSTRAINT [FK_Slide_SliderId] FOREIGN KEY ([SliderId]) REFERENCES [dbo].[Slider] ([SliderId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserCmsResource_ToCmsResource]...';


GO
ALTER TABLE [dbo].[UserCmsResource]
    ADD CONSTRAINT [FK_UserCmsResource_ToCmsResource] FOREIGN KEY ([CmsResourceId]) REFERENCES [dbo].[CmsResource] ([CmsResourceId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_UserCmsResource_ToUser]...';


GO
ALTER TABLE [dbo].[UserCmsResource]
    ADD CONSTRAINT [FK_UserCmsResource_ToUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE;



GO
PRINT N'Creating [dbo].[FK_OrderFormField_ToOrderForm]...';


GO
ALTER TABLE [dbo].[OrderFormField]
    ADD CONSTRAINT [FK_OrderFormField_ToOrderForm] FOREIGN KEY ([OrderFormId]) REFERENCES [dbo].[OrderForm] ([OrderFormId]) ON DELETE CASCADE;

GO
PRINT N'Creating [dbo].[FK_Video_Image]...';


GO
ALTER TABLE [dbo].[Video]
    ADD CONSTRAINT [FK_Video_Image] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON DELETE SET NULL ON UPDATE CASCADE;


GO
PRINT N'Creating [dbo].[FK_Video_VideoAlbum]...';


GO
ALTER TABLE [dbo].[Video]
    ADD CONSTRAINT [FK_Video_VideoAlbum] FOREIGN KEY ([AlbumId]) REFERENCES [dbo].[VideoAlbum] ([AlbumId]) ON DELETE CASCADE;


GO
PRINT N'Creating [dbo].[FK_VideoAlbum_User]...';


GO
ALTER TABLE [dbo].[VideoAlbum]
    ADD CONSTRAINT [FK_VideoAlbum_User] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User] ([UserId]);


GO
PRINT N'Creating [News].[FK_NewsTag_News]...';


GO
ALTER TABLE [News].[NewsItemTag]
    ADD CONSTRAINT [FK_NewsTag_News] FOREIGN KEY ([NewsItemId]) REFERENCES [News].[NewsItem] ([NewsId]) ON DELETE CASCADE;


GO
PRINT N'Creating [News].[FK_NewsItemTag_Tag]...';


GO
ALTER TABLE [News].[NewsItemTag]
    ADD CONSTRAINT [FK_NewsItemTag_Tag] FOREIGN KEY ([TagId]) REFERENCES [News].[Tag] ([TagId]) ON DELETE CASCADE;


GO
PRINT N'Creating [News].[FK_NewsItem_Category_News]...';


GO
ALTER TABLE [News].[NewsItem_Category]
    ADD CONSTRAINT [FK_NewsItem_Category_News] FOREIGN KEY ([NewsItemId]) REFERENCES [News].[NewsItem] ([NewsId]) ON DELETE CASCADE;


GO
PRINT N'Creating [News].[FK_NewsItem_Category_Category]...';


GO
ALTER TABLE [News].[NewsItem_Category]
    ADD CONSTRAINT [FK_NewsItem_Category_Category] FOREIGN KEY ([CategoryId]) REFERENCES [News].[Category] ([CategoryId]) ON DELETE CASCADE;


GO
PRINT N'Creating [News].[FK_Category_Parent]...';


GO
ALTER TABLE [News].[Category]
    ADD CONSTRAINT [FK_Category_Parent] FOREIGN KEY ([ParentCategoryId]) REFERENCES [News].[Category] ([CategoryId]);


GO
PRINT N'Creating [News].[FK_NewsSetImage]...';


GO
ALTER TABLE [News].[NewsItem]
    ADD CONSTRAINT [FK_NewsSetImage] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON DELETE SET NULL ON UPDATE CASCADE;


GO
PRINT N'Creating [News].[FK_NewsSetUser]...';


GO
ALTER TABLE [News].[NewsItem]
    ADD CONSTRAINT [FK_NewsSetUser] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User] ([UserId]);


GO
PRINT N'Creating [News].[FK_NewsSetBlog]...';


GO
ALTER TABLE [News].[NewsItem]
    ADD CONSTRAINT [FK_NewsSetBlog] FOREIGN KEY ([BlogId]) REFERENCES [News].[Blog] ([BlogId]);


GO
PRINT N'Creating [News].[FK_NewsSet_CommentTopic]...';


GO
ALTER TABLE [News].[NewsItem]
    ADD CONSTRAINT [FK_NewsSet_CommentTopic] FOREIGN KEY ([CommentTopicId]) REFERENCES [Comments].[CommentTopic] ([CommentTopicId]) ON DELETE SET NULL;


GO
PRINT N'Creating [News].[FK_Blog_User_User]...';


GO
ALTER TABLE [News].[Blog_User]
    ADD CONSTRAINT [FK_Blog_User_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE;


GO
PRINT N'Creating [News].[FK_Blog_User_Blog]...';


GO
ALTER TABLE [News].[Blog_User]
    ADD CONSTRAINT [FK_Blog_User_Blog] FOREIGN KEY ([BlogId]) REFERENCES [News].[Blog] ([BlogId]) ON DELETE CASCADE;


GO
PRINT N'Creating [News].[FK_Blog_User]...';


GO
ALTER TABLE [News].[Blog]
    ADD CONSTRAINT [FK_Blog_User] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User] ([UserId]);


GO
PRINT N'Creating [Comments].[FK_Comment_Author]...';


GO
ALTER TABLE [Comments].[Comment]
    ADD CONSTRAINT [FK_Comment_Author] FOREIGN KEY ([AuthorId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE SET NULL;


GO
PRINT N'Creating [Comments].[FK_Comment_Comment]...';


GO
ALTER TABLE [Comments].[Comment]
    ADD CONSTRAINT [FK_Comment_Comment] FOREIGN KEY ([ParentCommentId]) REFERENCES [Comments].[Comment] ([CommentId]);


GO
PRINT N'Creating [Comments].[FK_Comment_CommentTopic]...';


GO
ALTER TABLE [Comments].[Comment]
    ADD CONSTRAINT [FK_Comment_CommentTopic] FOREIGN KEY ([CommentTopicId]) REFERENCES [Comments].[CommentTopic] ([CommentTopicId]);


GO
PRINT N'Creating [Comments].[CommentDelete]...';


GO
CREATE TRIGGER [Comments].[CommentDelete]
	ON [Comments].[Comment]
	INSTEAD OF DELETE
	AS
	BEGIN
		DELETE FROM [Comments].[Comment]
			WHERE [ParentCommentId] IN (SELECT deleted.[CommentId] FROM deleted)
		DELETE FROM [Comments].[Comment]
			WHERE [CommentId] IN (SELECT deleted.[CommentId] FROM deleted)
		
	END
GO
PRINT N'Creating [Comments].[CommentTopicDelete]...';


GO
CREATE TRIGGER [Comments].[CommentTopicDelete]
	ON [Comments].[CommentTopic]
	INSTEAD OF DELETE
	AS
	BEGIN
		DELETE FROM [Comments].[Comment]
			WHERE [CommentTopicId] IN (SELECT deleted.[CommentTopicId] FROM deleted)
		DELETE FROM [Comments].[CommentTopic]
			WHERE [CommentTopicId] IN (SELECT deleted.[CommentTopicId] FROM deleted)
	END
GO
PRINT N'Creating [dbo].[f_BinaryToBase64]...';


GO
CREATE FUNCTION dbo.f_BinaryToBase64(@bin VARBINARY(MAX))
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Base64 VARCHAR(MAX)
    
    /*
        SELECT dbo.f_BinaryToBase64(CONVERT(VARBINARY(MAX), 'Converting this text to Base64...'))
    */
    
    SET @Base64 = CAST(N'' AS XML).value('xs:base64Binary(xs:hexBinary(sql:variable("@bin")))','VARCHAR(MAX)')

	-- Replacement for URLs!!!
    SET @Base64 = REPLACE(@Base64, '+', '-')
	SET @Base64 = REPLACE(@Base64, '/', '_')
	SET @Base64 = REPLACE(@Base64, '=', '')
    RETURN @Base64
END
GO
PRINT N'Creating [dbo].[Album_Delete]...';


GO
CREATE PROCEDURE [dbo].[Album_Delete]
@AlbumId int
AS
	DELETE FROM [dbo].[Album]
	WHERE [AlbumId]=@AlbumId
GO
PRINT N'Creating [dbo].[Album_Insert]...';


GO
CREATE PROCEDURE [dbo].[Album_Insert]
@Name nvarchar(50),
@Description nvarchar(MAX),
@OwnerId int
AS
	INSERT INTO [dbo].[Album] ([Name], [Description], [OwnerId])
	VALUES (@Name, @Description, @OwnerId)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Album_Select]...';


GO
CREATE PROCEDURE [dbo].[Album_Select]
AS
	SELECT [AlbumId], [Name], [Description], [CreationDate], [OwnerId] FROM [dbo].[Album]
GO
PRINT N'Creating [dbo].[Album_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Album_SelectOne]
	@AlbumId int
AS
	SELECT * FROM [Album] WHERE [AlbumId]=@AlbumId
GO
PRINT N'Creating [dbo].[Album_SelectUserAlbums]...';


GO
CREATE PROCEDURE [dbo].[Album_SelectUserAlbums]
	@OwnerId int
AS
	SELECT * FROM [Album] 
		WHERE (@OwnerId IS NULL AND [OwnerId] IS NOT NULL) OR (OwnerId = @OwnerId)
GO
PRINT N'Creating [dbo].[Album_Update]...';


GO
CREATE PROCEDURE [dbo].[Album_Update]
@Name nvarchar(50),
@Description nvarchar(MAX),
@OwnerId int,
@WatermarkImageId varchar(30),
@AlbumId int
AS
	UPDATE [dbo].[Album] SET
		[Name]=@Name,
		[Description]=@Description,
		[OwnerId]=@OwnerId,
		[WatermarkImageId]=@WatermarkImageId
	WHERE [AlbumId]=@AlbumId
GO
PRINT N'Creating [dbo].[Block_Delete]...';


GO
CREATE PROCEDURE [dbo].[Block_Delete]
@BlockId int
AS
	DELETE FROM [dbo].[Block]
	WHERE [BlockId]=@BlockId
GO
PRINT N'Creating [dbo].[Block_Insert]...';


GO
CREATE PROCEDURE [dbo].[Block_Insert]
@Title nvarchar(max),
@Content nvarchar(max)
AS
	INSERT INTO [dbo].[Block] ([Title], [Content])
	VALUES (@Title, @Content)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Block_Select]...';


GO
CREATE PROCEDURE [dbo].[Block_Select]
AS
	SELECT [BlockId], [Title], [Content] FROM [dbo].[Block]
GO
PRINT N'Creating [dbo].[Block_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Block_SelectOne]
@BlockId int
AS
	SELECT [BlockId], [Title], [Content] FROM [dbo].[Block]
	WHERE [BlockId]=@BlockId
GO
PRINT N'Creating [dbo].[Block_Update]...';


GO
CREATE PROCEDURE [dbo].[Block_Update]
@BlockId int,
@Title nvarchar(max),
@Content nvarchar(max)
AS
	UPDATE [dbo].[Block]
	SET [Title]=@Title, 
		[Content]=@Content
	WHERE [BlockId]=@BlockId
GO
PRINT N'Creating [dbo].[CmsResource_Delete]...';


GO
CREATE PROCEDURE [dbo].[CmsResource_Delete]
@CmsResourceId int
AS
	DELETE FROM [dbo].[CmsResource]
	WHERE [CmsResourceId]=@CmsResourceId
GO
PRINT N'Creating [dbo].[CmsResource_Insert]...';


GO
CREATE PROCEDURE [dbo].[CmsResource_Insert]
@Name varchar(50),
@Description nvarchar(200)
AS
	INSERT INTO [dbo].[CmsResource] ([Name], [Description])
	VALUES (@Name, @Description)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[CmsResource_Select]...';


GO
CREATE PROCEDURE [dbo].[CmsResource_Select]
AS
	SELECT [CmsResourceId], [Name], [Description] FROM [dbo].[CmsResource]
GO
PRINT N'Creating [dbo].[CmsResource_SelectByName]...';


GO
CREATE PROCEDURE [dbo].[CmsResource_SelectByName]
	@Name varchar(50)
AS
	SELECT * FROM [CmsResource] WHERE [Name]=@Name
GO
PRINT N'Creating [dbo].[CmsResource_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[CmsResource_SelectOne]
	@CmsResourceId int
AS
	SELECT [CmsResourceId], [Name], [Description] FROM [dbo].[CmsResource]
		WHERE [CmsResourceId]=@CmsResourceId
GO
PRINT N'Creating [dbo].[CmsResource_Update]...';


GO
CREATE PROCEDURE [dbo].[CmsResource_Update]
@Name varchar(50),
@Description nvarchar(200),
@CmsResourceId int
AS
	UPDATE [dbo].[CmsResource] SET
		[Name]=@Name,
		[Description]=@Description
	WHERE [CmsResourceId]=@CmsResourceId
GO
PRINT N'Creating [dbo].[FormRequest_Delete]...';


GO
CREATE PROCEDURE [dbo].[FormRequest_Delete]
	@FormRequestId INT
AS
	DELETE FROM [dbo].[FormRequest]
	WHERE [FormRequestId] = @FormRequestId
GO
PRINT N'Creating [dbo].[FormRequest_Insert]...';


GO
CREATE PROCEDURE [dbo].[FormRequest_Insert]
@Name NVARCHAR(50), 
@Email NVARCHAR(50), 
@Phone NVARCHAR(50), 
@Text NVARCHAR(MAX)
AS
	INSERT INTO [dbo].[FormRequest] ([Name], [Email], [Phone], [Text])
	VALUES (@Name, @Email, @Phone, @Text)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[FormRequest_Select]...';


GO
CREATE PROCEDURE [dbo].[FormRequest_Select]
AS
	SELECT [FormRequestId], [Name], [Email], [Phone], [Text], [CreationDate]
	FROM [dbo].[FormRequest]
	ORDER BY [CreationDate] DESC
GO
PRINT N'Creating [dbo].[FormRequest_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[FormRequest_SelectOne]
	@FormRequestId INT
AS
	SELECT [FormRequestId], [Name], [Email], [Phone], [Text], [CreationDate]
	FROM [dbo].[FormRequest]
	WHERE [FormRequestId] = @FormRequestId
GO
PRINT N'Creating [dbo].[FormRequest_Update]...';


GO
CREATE PROCEDURE [dbo].[FormRequest_Update]
@FormRequestId INT,
@Name NVARCHAR(50), 
@Email NVARCHAR(50), 
@Phone NVARCHAR(50), 
@Text NVARCHAR(MAX)
AS
	UPDATE [dbo].[FormRequest]
	SET [Name] = @Name,
		[Email] = @Email,
		[Phone] = @Phone,
		[Text] = @Text
	WHERE [FormRequestId] = @FormRequestId
GO
PRINT N'Creating [dbo].[Image_CheckIfExists]...';


GO
CREATE PROCEDURE [dbo].[Image_CheckIfExists]
	@ImageId varchar(30)
AS
	IF EXISTS( SELECT * FROM Image WHERE [ImageId]=@ImageId )
		SELECT 1
	ELSE
		SELECT 0
GO
PRINT N'Creating [dbo].[Image_Delete]...';


GO
CREATE PROCEDURE [dbo].[Image_Delete]
@ImageId varchar(30)
AS
	DELETE FROM [dbo].[Image]
	WHERE [ImageId]=@ImageId
GO
PRINT N'Creating [dbo].[Image_Insert]...';


GO
CREATE PROCEDURE [dbo].[Image_Insert]
@ImageId varchar(30),
@InitialFilename nvarchar(257)
AS
	INSERT INTO [dbo].[Image] ([ImageId], [InitialFilename])
	VALUES (@ImageId, @InitialFilename)
GO
PRINT N'Creating [dbo].[Image_Select]...';


GO
CREATE PROCEDURE [dbo].[Image_Select]
AS
	SELECT [ImageId], [CreationDate], [InitialFilename] FROM [dbo].[Image]
GO
PRINT N'Creating [dbo].[Image_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Image_SelectOne]
	@ImageId varchar(30)
AS
	SELECT * FROM Image WHERE ImageId=@ImageId
GO
PRINT N'Creating [dbo].[Image_Update]...';


GO
CREATE PROCEDURE [dbo].[Image_Update]
	@ImageId varchar(30),
	@InitialFilename nvarchar(257)
AS
	UPDATE Image SET
		[InitialFilename]=@InitialFilename
		WHERE [ImageId]=@ImageId
GO
PRINT N'Creating [dbo].[ImageInAlbum_Delete]...';


GO
CREATE PROCEDURE [dbo].[ImageInAlbum_Delete]
@AlbumId int,
@ImageId varchar(30)
AS
	DELETE FROM [dbo].[ImageInAlbum]
	WHERE [AlbumId]=@AlbumId
		 AND [ImageId]=@ImageId
GO
PRINT N'Creating [dbo].[ImageInAlbum_Insert]...';


GO
CREATE PROCEDURE [dbo].[ImageInAlbum_Insert]
@AlbumId int,
@ImageId varchar(30),
@Title nvarchar(MAX),
@Description nvarchar(MAX),
@SortOrder int,
@DestinationUrl nvarchar(250)
AS
	INSERT INTO [dbo].[ImageInAlbum] ([AlbumId], [ImageId], [Title], [Description], [SortOrder], [DestinationUrl])
	VALUES (@AlbumId, @ImageId, @Title, @Description, @SortOrder, @DestinationUrl)
GO
PRINT N'Creating [dbo].[ImageInAlbum_SelectByAlbum]...';


GO
CREATE PROCEDURE [dbo].[ImageInAlbum_SelectByAlbum]
	@AlbumId int
AS
	SELECT * FROM [ImageInAlbum] WHERE [AlbumId]=@AlbumId
PRINT N'Creating [dbo].[ImageInAlbum_SelectByImage]...';
GO

CREATE PROCEDURE [dbo].[ImageInAlbum_SelectByImage]
	@ImageId varchar(30)
AS
	SELECT * FROM [ImageInAlbum] WHERE [ImageId]=@ImageId
PRINT N'Creating [dbo].[ImageInAlbum_SelectCountByAlbum]...';

GO
CREATE PROCEDURE [dbo].[ImageInAlbum_SelectCountByAlbum]
	@AlbumId int
AS
	SELECT COUNT(*) FROM [ImageInAlbum] WHERE [AlbumId]=@AlbumId
GO
PRINT N'Creating [dbo].[ImageInAlbum_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[ImageInAlbum_SelectOne]
	@AlbumId int,
	@ImageId varchar(30)
AS
	SELECT * FROM [ImageInAlbum] WHERE [AlbumId]=@AlbumId AND [ImageId]=@ImageId
GO
PRINT N'Creating [dbo].[ImageInAlbum_Update]...';


GO
CREATE PROCEDURE [dbo].[ImageInAlbum_Update]
@AlbumId int,
@ImageId varchar(30),
@Title nvarchar(MAX),
@Description nvarchar(MAX),
@SortOrder int,
@DestinationUrl nvarchar(250)
AS
	UPDATE [ImageInAlbum] SET
		[Title]=@Title,
		[Description]=@Description,
		[SortOrder]=@SortOrder,
		[DestinationUrl]=@DestinationUrl
		WHERE [AlbumId]=@AlbumId AND [ImageId]=@ImageId
GO
PRINT N'Creating [dbo].[InterfaceString_Delete]...';


GO
CREATE PROCEDURE [dbo].[InterfaceString_Delete]
	@Key varchar(200)
AS
	DELETE FROM InterfaceString WHERE [Key]=@Key
GO
PRINT N'Creating [dbo].[InterfaceString_Select]...';


GO
CREATE PROCEDURE [dbo].[InterfaceString_Select]
AS
	SELECT * FROM InterfaceString
GO
PRINT N'Creating [dbo].[InterfaceString_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[InterfaceString_SelectOne]
	@Key varchar(200)
AS
	SELECT * FROM InterfaceString WHERE [Key]=@Key
GO
PRINT N'Creating [dbo].[InterfaceString_Upsert]...';


GO
CREATE PROCEDURE [dbo].[InterfaceString_Upsert]
	@Key varchar(200),
	@Value nvarchar(max)
AS
	IF EXISTS(SELECT * FROM InterfaceString WHERE [Key]=@Key)
		UPDATE InterfaceString SET [Value]=@Value WHERE [Key]=@Key
	ELSE
		INSERT INTO InterfaceString ([Key],[Value]) VALUES (@Key, @Value)
GO
PRINT N'Creating [dbo].[Mail_Delete]...';


GO
CREATE PROCEDURE [dbo].[Mail_Delete]
	@MailId int
AS
	delete from [dbo].[Mail]
	where [MailId] = @MailId
GO
PRINT N'Creating [dbo].[Mail_Insert]...';


GO
CREATE PROCEDURE [dbo].[Mail_Insert]
	@Body nvarchar(MAX),
	@Subject nvarchar(MAX),
	@Receiver nvarchar(MAX),
	@Sent bit,
	@ErrorMessage nvarchar(MAX),
	@Attaches nvarchar(MAX)
AS
	insert into [dbo].[Mail] ([Body], [Subject], [Receiver], [Sent], [ErrorMessage], [Attaches])
	values (@Body, @Subject, @Receiver, @Sent, @ErrorMessage, @Attaches)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Mail_Select]...';


GO
CREATE PROCEDURE [dbo].[Mail_Select]
AS
	SELECT [MailId], [CreationDate], [Body], [Subject], [Receiver], [Sent], [ErrorMessage], [Attaches]
	from [dbo].[Mail]
	order by  [CreationDate] desc
GO
PRINT N'Creating [dbo].[Mail_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Mail_SelectOne]
	@MailId int
AS
	SELECT [MailId], [CreationDate], [Body], [Subject], [Receiver], [Sent], [ErrorMessage], [Attaches]
	from [dbo].[Mail]
	where [MailId] = @MailId
GO
PRINT N'Creating [dbo].[Mail_Update]...';


GO
CREATE PROCEDURE [dbo].[Mail_Update]
	@MailId int,
	@Body nvarchar(MAX),
	@Subject nvarchar(MAX),
	@Receiver nvarchar(MAX),
	@ErrorMessage nvarchar(MAX),
	@Sent bit,
	@Attaches nvarchar(MAX)
AS
	update [dbo].[Mail] 
	set [Body] = @Body, 
		[Subject] = @Subject, 
		[Receiver] = @Receiver, 
		[Sent] = @Sent, 
		[ErrorMessage] = @ErrorMessage,
		[Attaches] = @Attaches
	where [MailId] = @MailId
RETURN 0
GO
PRINT N'Creating [dbo].[Menu_Delete]...';


GO
CREATE PROCEDURE [dbo].[Menu_Delete]
	@MenuId int
AS
	DELETE FROM Menu
	WHERE MenuId = @MenuId
GO
PRINT N'Creating [dbo].[Menu_Insert]...';


GO
CREATE PROCEDURE [dbo].[Menu_Insert]
	@Name nvarchar(200)
AS

INSERT INTO Menu ([Name]) Values (@Name)
SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Menu_Select]...';


GO
CREATE PROCEDURE [dbo].[Menu_Select]

AS
	SELECT * FROM Menu
GO
PRINT N'Creating [dbo].[Menu_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Menu_SelectOne]
	@MenuId int
AS
	SELECT * FROM Menu WHERE MenuId = @MenuId
GO
PRINT N'Creating [dbo].[Menu_Update]...';


GO
CREATE PROCEDURE [dbo].[Menu_Update]
	@MenuId int,
	@Name nvarchar(200)
AS
	UPDATE Menu
	SET Name = @Name
	WHERE MenuId = @MenuId
GO
PRINT N'Creating [dbo].[MenuItem_Delete]...';


GO
CREATE PROCEDURE [dbo].[MenuItem_Delete]
	@MenuItemId int

	AS

	DELETE FROM MenuItem
	WHERE MenuItemId=@MenuItemId
GO
PRINT N'Creating [dbo].[MenuItem_Insert]...';


GO
CREATE PROCEDURE [dbo].[MenuItem_Insert]
    @Name NVARCHAR (200),
    @MenuId  INT,
    @ParentMenuItemId INT,
    @PageUrl NVARCHAR(300),
    @SortOrder INT,
    @BlockId INT

AS

INSERT INTO MenuItem ([Name], [MenuId], [ParentMenuItemId], [PageUrl], [SortOrder],[BlockId])
VALUES (@Name, @MenuId, @ParentMenuItemId, @PageUrl, @SortOrder, @BlockId)
SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[MenuItem_Select]...';


GO
CREATE PROCEDURE [dbo].[MenuItem_Select]
	@MenuId int

AS

SELEcT * FROM MenuItem
WHERE MenuId=@MenuId
GO
PRINT N'Creating [dbo].[MenuItem_SelectChildren]...';


GO
CREATE PROCEDURE [dbo].[MenuItem_SelectChildren]
	@MenuItemId int

AS

SELECT * FROM MenuItem
WHERE ParentMenuItemId=@MenuItemId
GO
PRINT N'Creating [dbo].[MenuItem_Update]...';


GO
CREATE PROCEDURE [dbo].[MenuItem_Update]
	@MenuItemId int,
	@Name NVARCHAR (200),
    @MenuId  INT,
    @ParentMenuItemId INT,
    @PageUrl NVARCHAR(300),
    @SortOrder INT,
    @BlockId INT
AS

UPDATE MenuItem
SET Name=@Name,
	MenuId=@MenuId,
	ParentMenuItemId=@ParentMenuItemId,
	PageUrl=@PageUrl,
	SortOrder=@SortOrder,
	BlockId=@BlockId
WHERE MenuItemId=@MenuItemId
GO
PRINT N'Creating [dbo].[Page_Delete]...';


GO
CREATE PROCEDURE [dbo].[Page_Delete]
	@PageId int
AS
	DELETE FROM [Page] WHERE [PageId]=@PageId
GO
PRINT N'Creating [dbo].[Page_Insert]...';


GO
CREATE PROCEDURE [dbo].[Page_Insert]
@Title nvarchar(MAX),
@Annotation nvarchar(MAX),
@Content nvarchar(MAX),
@RelativeUrl nvarchar(300),
@Keywords nvarchar(MAX),
@ParentPageId int,
@HideInSitemap bit,
@Header nvarchar(MAX),
@Styles nvarchar(MAX),
@Scripts nvarchar(MAX),
@Layout varchar(300),
@AdditionalHeaders nvarchar(MAX)
AS
	INSERT INTO [dbo].[Page] ([Title], [Annotation], [Content], [RelativeUrl], [Keywords], [ParentPageId], [HideInSitemap], [Header], [Styles], [Scripts], [Layout], [AdditionalHeaders])
	VALUES (@Title, @Annotation, @Content, @RelativeUrl, @Keywords, @ParentPageId, @HideInSitemap, @Header, @Styles, @Scripts, @Layout, @AdditionalHeaders)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Page_Select]...';


GO
CREATE PROCEDURE [dbo].[Page_Select]
AS
	SELECT * FROM Page
GO
PRINT N'Creating [dbo].[Page_SelectByRelativeUrl]...';


GO
CREATE PROCEDURE [dbo].[Page_SelectByRelativeUrl]
	@RelativeUrl nvarchar(300)
AS
	SELECT * FROM [Page] WHERE [RelativeUrl]=@RelativeUrl
GO
PRINT N'Creating [dbo].[Page_SelectChildren]...';


GO
CREATE PROCEDURE [dbo].[Page_SelectChildren]
	@ParentPageId int
AS
	SELECT * FROM [Page] WHERE [ParentPageId]=@ParentPageId
GO
PRINT N'Creating [dbo].[Page_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Page_SelectOne]
	@PageId int
AS
	SELECT * FROM Page WHERE [PageId]=@PageId
GO
PRINT N'Creating [dbo].[Page_Update]...';


GO
CREATE PROCEDURE [dbo].[Page_Update]
@Title nvarchar(MAX),
@Annotation nvarchar(MAX),
@Content nvarchar(MAX),
@RelativeUrl nvarchar(300),
@Keywords nvarchar(MAX),
@ParentPageId int,
@HideInSitemap bit,
@Header nvarchar(MAX),
@Styles nvarchar(MAX),
@Scripts nvarchar(MAX),
@Layout varchar(300),
@AdditionalHeaders nvarchar(MAX),
@PageId int
AS
	UPDATE [dbo].[Page] SET
		[Title]=@Title,
		[Annotation]=@Annotation,
		[Content]=@Content,
		[RelativeUrl]=@RelativeUrl,
		[Keywords]=@Keywords,
		[ParentPageId]=@ParentPageId,
		[HideInSitemap]=@HideInSitemap,
		[Header]=@Header,
		[Styles]=@Styles,
		[Scripts]=@Scripts,
		[Layout]=@Layout,
		[AdditionalHeaders]=@AdditionalHeaders
	WHERE [PageId]=@PageId
GO
PRINT N'Creating [dbo].[PasswordTicket_Insert]...';


GO
CREATE PROCEDURE [dbo].[PasswordTicket_Insert]
	@UserId int,
	@Token uniqueidentifier,
	@ExpirationDate datetime
AS
	INSERT INTO [PasswordTicket] (UserId,Token,ExpirationDate) VALUES (@UserId, @Token, @ExpirationDate)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[PasswordTicket_SelectAvailableByToken]...';


GO
CREATE PROCEDURE [dbo].[PasswordTicket_SelectAvailableByToken]
	@Token uniqueidentifier
AS
	SELECT * FROM [PasswordTicket] 
		WHERE [Token]=@Token AND [ExpirationDate]>=GETUTCDATE() AND [Used]=0
GO
PRINT N'Creating [dbo].[PasswordTicket_UseTicket]...';


GO
CREATE PROCEDURE [dbo].[PasswordTicket_UseTicket]
	@TicketId int
AS
	UPDATE PasswordTicket SET
		[Used]=1,
		[UseDate]=GETUTCDATE()
		WHERE [TicketId]=@TicketId
GO
PRINT N'Creating [dbo].[Review_Delete]...';


GO
CREATE PROCEDURE [dbo].[Review_Delete]
@ReviewId int
AS
	DELETE FROM [dbo].[Review]
	WHERE [ReviewId]=@ReviewId
GO
PRINT N'Creating [dbo].[Review_Insert]...';


GO
CREATE PROCEDURE [dbo].[Review_Insert]
@Author    NVARCHAR (50), 
@City      NVARCHAR (50), 
@Email     NVARCHAR (100),
@Text      NVARCHAR (MAX),	
@Moderated BIT,           	
@VK 	   NVARCHAR(50)
AS
	INSERT INTO [dbo].[Review] ([Author], [City], [Email], [Text], [Moderated], [VK] )
	VALUES (@Author, @City, @Email, @Text, @Moderated, @VK )
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Review_Select]...';


GO
CREATE PROCEDURE [dbo].[Review_Select]
@Start INT,
@Count INT,
@Total INT OUT,
@OnlyModerated BIT
AS
    SELECT @Total = COUNT(*) 
	FROM [dbo].[Review]
	WHERE [Moderated] = 1

	SELECT [ReviewId], [Author], [City], [Email], [Text], [Moderated], [VK], [CreationDate] 
	FROM [dbo].[Review]
	WHERE @OnlyModerated = 0 or [Moderated] = 1
	ORDER BY [CreationDate] DESC
	OFFSET @Start - 1   ROWS
	FETCH NEXT @Count ROWS ONLY
GO
PRINT N'Creating [dbo].[Review_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Review_SelectOne]
@ReviewId INT
AS
	SELECT [ReviewId], [Author], [City], [Email], [Text], [Moderated], [VK], [CreationDate] 
	FROM [dbo].[Review]
	WHERE [ReviewId] = @ReviewId
GO
PRINT N'Creating [dbo].[Review_Update]...';


GO
CREATE PROCEDURE [dbo].[Review_Update]
@ReviewId INT,
@Author    NVARCHAR (50), 
@City      NVARCHAR (50), 
@Email     NVARCHAR (100),
@Text      NVARCHAR (MAX),	
@Moderated BIT,           	
@VK 	   NVARCHAR(50)
AS
	UPDATE [dbo].[Review]
	set [Author] = @Author,
        [City] = @City,
		[Email] = @Email,
		[Text] = @Text, 
		[Moderated] = @Moderated,
		[VK] = @VK 
    WHERE [ReviewId] = @ReviewId
GO
PRINT N'Creating [dbo].[SearchItem_Delete]...';


GO
CREATE PROCEDURE [dbo].[SearchItem_Delete]
	@EntityName varchar(200),
	@EntityId nvarchar(100)
AS
	DELETE FROM SearchItem WHERE [EntityName]=@EntityName AND [EntityId]=@EntityId
GO
PRINT N'Creating [dbo].[SearchItem_Find]...';


GO
CREATE PROCEDURE [dbo].[SearchItem_Find]
	@FulltextSearchQuery nvarchar(2000),
	@Entities String_Table readonly
AS
	IF (@FulltextSearchQuery IS NOT NULL AND @FulltextSearchQuery != '')
		SELECT s.SearchItemKey, s.[EntityName], s.[EntityId], s.[Text], s.Title, s.[Url], s.[Weight], s.[ImageId], tbl.[RANK] as [Rank]
			FROM [SearchItem] s JOIN FREETEXTTABLE([SearchItem], [SearchContent], @FulltextSearchQuery) tbl
			ON s.[FulltextKey]=tbl.[KEY]
			WHERE
				(NOT EXISTS (SELECT * FROM @Entities) OR (s.EntityName IN (SELECT Val FROM @Entities)))
			ORDER BY [Rank] DESC
	ELSE
		SELECT s.SearchItemKey, s.[EntityName], s.[EntityId], s.[Text], s.Title, s.[Url], s.[Weight], s.[ImageId], 0 as [Rank]
			FROM [SearchItem] s
			WHERE
				(NOT EXISTS (SELECT * FROM @Entities) OR (s.EntityName IN (SELECT Val FROM @Entities)))
GO
PRINT N'Creating [dbo].[SearchItem_Upsert]...';


GO
CREATE PROCEDURE [dbo].[SearchItem_Upsert]
	@SearchItemKey varchar(50),
	@EntityName varchar(200),
	@EntityId nvarchar(100),
	@SearchContent nvarchar(max),
	@Text nvarchar(500),
	@Title nvarchar(200),
	@Url nvarchar(500),
	@ImageId VARCHAR(30),
	@Weight int
AS
	IF (EXISTS(SELECT * FROM SearchItem WHERE [SearchItemKey]=@SearchItemKey AND [EntityName]=@EntityName AND [EntityId]=@EntityId))
		UPDATE [SearchItem]
			SET [SearchContent]=@SearchContent,
			[Text]=@Text,
			[Title]=@Title,
			[Url]=@Url,
			[Weight]=@Weight,
			[ImageId]=@ImageId,
			[UpdateDate]=GETUTCDATE()
		WHERE 
			[SearchItemKey]=@SearchItemKey AND [EntityName]=@EntityName AND [EntityId]=@EntityId
	ELSE
		INSERT INTO [SearchItem] ([SearchItemKey], [EntityName], [EntityId], [SearchContent], [Text], [Title], [Url], [Weight],[ImageId])
			VALUES (@SearchItemKey, @EntityName, @EntityId, @SearchContent, @Text, @Title, @Url, @Weight, @ImageId)
GO
PRINT N'Creating [dbo].[Setting_Delete]...';


GO
CREATE PROCEDURE [dbo].[Setting_Delete]
	@Key nvarchar(100)
AS
	DELETE FROM [Setting] WHERE [Key]=@Key
GO
PRINT N'Creating [dbo].[Setting_GetValue]...';


GO
CREATE PROCEDURE [dbo].[Setting_GetValue]
	@Key NVARCHAR (100)
AS
	SELECT [Value] FROM [Setting] WHERE [Key]=@Key
GO
PRINT N'Creating [dbo].[Setting_Select]...';


GO
CREATE PROCEDURE [dbo].[Setting_Select]
AS
	SELECT * FROM Setting
GO
PRINT N'Creating [dbo].[Setting_Upsert]...';


GO
CREATE PROCEDURE [dbo].[Setting_Upsert]
	@Key NVARCHAR (100),
	@Value NVARCHAR (MAX)
AS
	IF (EXISTS (SELECT * FROM [Setting] WHERE [Key]=@Key))
		UPDATE [Setting] SET [Value]=@Value
			WHERE [Key]=@Key
	ELSE
		INSERT INTO [Setting] ([Key],[Value]) VALUES (@Key, @Value)
GO
PRINT N'Creating [dbo].[Slide_Delete]...';


GO
CREATE PROCEDURE [dbo].[Slide_Delete]
@SlideId int
AS
    DELETE FROM [dbo].[Slide]
    WHERE [SlideId]=@SlideId
GO
PRINT N'Creating [dbo].[Slide_Insert]...';


GO
CREATE PROCEDURE [dbo].[Slide_Insert]
@SliderId    INT,
@Title       NVARCHAR (50),
@Description NVARCHAR (1000),
@ImageId     VARCHAR(30),
@Link        NVARCHAR (MAX)
AS
    INSERT INTO [dbo].[Slide] ([SliderId], [Title], [Description], [ImageId], [Link])
    VALUES (@SliderId, @Title, @Description, @ImageId, @Link)
    SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Slide_Select]...';


GO
CREATE PROCEDURE [dbo].[Slide_Select]
@SliderId INT
AS
    SELECT [SlideId], [SliderId], [Title], [Description], [ImageId], [Link], [SortOrder]
    FROM [dbo].[Slide]
	WHERE [SliderId]=@SliderId
GO
PRINT N'Creating [dbo].[Slide_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Slide_SelectOne]
@SlideId INT
AS
    SELECT [SlideId], [SliderId], [Title], [Description], [ImageId], [Link], [SortOrder]
    FROM [dbo].[Slide]
    WHERE [SlideId]=@SlideId
GO
PRINT N'Creating [dbo].[Slide_Update]...';


GO
CREATE PROCEDURE [dbo].[Slide_Update]
@SlideId    INT,
@SliderId    INT,
@Title       NVARCHAR (50),
@Description NVARCHAR (1000),
@ImageId     VARCHAR(30),
@Link        NVARCHAR (MAX)
AS
    UPDATE [dbo].[Slide]
    SET [SliderId]    =@SliderId,
        [Title]       =@Title,
        [Description] =@Description,
        [ImageId]     =@ImageId,
        [Link]        =@Link
    WHERE [SlideId]=@SlideId
GO
PRINT N'Creating [dbo].[Slider_Delete]...';


GO
CREATE PROCEDURE [dbo].[Slider_Delete]
@SliderId int
AS
    DELETE FROM [dbo].[Slider]
    WHERE [SliderId]=@SliderId
GO
PRINT N'Creating [dbo].[Slider_Insert]...';


GO
CREATE PROCEDURE [dbo].[Slider_Insert]
@Name nvarchar(50)
AS
    INSERT INTO [dbo].[Slider] ([Name])
    VALUES (@Name)
    SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[Slider_Select]...';


GO
CREATE PROCEDURE [dbo].[Slider_Select]
AS
    SELECT [SliderId], [Name]
    FROM [dbo].[Slider]
GO
PRINT N'Creating [dbo].[Slider_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Slider_SelectOne]
@SliderId int
AS
    SELECT [SliderId], [Name]
    FROM [dbo].[Slider]
    WHERE [SliderId]=@SliderId
GO
PRINT N'Creating [dbo].[Slider_Update]...';


GO
CREATE PROCEDURE [dbo].[Slider_Update]
@SliderId int,
@Name nvarchar(50)
AS
    UPDATE [dbo].[Slider]
    SET [Name]=@Name
    WHERE [SliderId]=@SliderId
GO
PRINT N'Creating [dbo].[User_Authenticate]...';


GO
CREATE PROCEDURE [dbo].[User_Authenticate]
    @Username nvarchar(50), 
    @PassHash nvarchar(200)
AS
	IF (SELECT COUNT(*) FROM [dbo].[User] WHERE LOWER(Username)=LOWER(@Username) AND Password=@PassHash) = 0
		SELECT 0
	ELSE
		SELECT 1
GO
PRINT N'Creating [dbo].[User_Delete]...';


GO
CREATE PROCEDURE [dbo].[User_Delete]
@UserId int
AS
	DELETE FROM [dbo].[User]
	WHERE [UserId]=@UserId
GO
PRINT N'Creating [dbo].[User_Insert]...';


GO
CREATE PROCEDURE [dbo].[User_Insert]
@Username nvarchar(50),
@Password nvarchar(200),
@Email nvarchar(100),
@EmailConfirmed bit,
@Description nvarchar(1000),
@Vk nvarchar(100),
@Fb nvarchar(100),
@GoogleP nvarchar(100),
@Twitter nvarchar(100)
AS
	INSERT INTO [dbo].[User] ([Username], [Password], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter])
	VALUES (@Username, @Password, @Email, @EmailConfirmed, @Description, @Vk, @Fb, @GoogleP, @Twitter)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[User_Select]...';


GO
CREATE PROCEDURE [dbo].[User_Select]
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]
GO
PRINT N'Creating [dbo].[User_SelectByEmail]...';


GO
CREATE PROCEDURE [dbo].[User_SelectByEmail]
	@Email nvarchar(100)
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]
	WHERE [Email]=@Email
GO
PRINT N'Creating [dbo].[User_SelectByUsername]...';


GO
CREATE PROCEDURE [dbo].[User_SelectByUsername]
	@Username nvarchar(50)
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]
	WHERE [Username]=@Username
GO
PRINT N'Creating [dbo].[User_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[User_SelectOne]
	@UserId int
AS
	SELECT [UserId], [CreationDate], [Username], [Email], [EmailConfirmed], [Description], [Vk], [Fb], [GoogleP], [Twitter] FROM [dbo].[User]
	WHERE [UserId]=@UserId
GO
PRINT N'Creating [dbo].[User_Update]...';


GO
CREATE PROCEDURE [dbo].[User_Update]
@Email nvarchar(100),
@EmailConfirmed bit,
@Description nvarchar(1000),
@Vk nvarchar(100),
@Fb nvarchar(100),
@GoogleP nvarchar(100),
@Twitter nvarchar(100),
@UserId int
AS
	UPDATE [User] SET
		[Email]=@Email,
		[EmailConfirmed]=@EmailConfirmed,
		[Description]=@Description,
		[Vk]=@Vk,
		[Fb]=@Fb,
		[GoogleP]=@GoogleP,
		[Twitter]=@Twitter
		WHERE [UserId]=@UserId
GO
PRINT N'Creating [dbo].[User_UpdatePassword]...';


GO
CREATE PROCEDURE [dbo].[User_UpdatePassword]
    @UserId int,
    @PassHash nvarchar(200)
AS
	UPDATE [dbo].[User] SET [Password] = @PassHash WHERE [UserId]=@UserId
GO
PRINT N'Creating [dbo].[UserCmsResource_CheckIfAuthorizedForResource]...';


GO
CREATE PROCEDURE [dbo].[UserCmsResource_CheckIfAuthorizedForResource]
	@UserId int,
	@ResourceName varchar(50)
AS
	SELECT COUNT(*) FROM [UserCmsResource] ucr JOIN [CmsResource] cr ON ucr.CmsResourceId=cr.CmsResourceId 
		WHERE ucr.UserId=@UserId AND cr.Name=@ResourceName
GO
PRINT N'Creating [dbo].[UserCmsResource_Delete]...';


GO
CREATE PROCEDURE [dbo].[UserCmsResource_Delete]
	@UserId int,
	@CmsResourceId int
AS
	DELETE FROM [UserCmsResource] WHERE [UserId]=@UserId AND [CmsResourceId]=@CmsResourceId
GO
PRINT N'Creating [dbo].[UserCmsResource_Insert]...';


GO
CREATE PROCEDURE [dbo].[UserCmsResource_Insert]
	@UserId int,
	@CmsResourceId int
AS
	INSERT INTO [UserCmsResource] (UserId, CmsResourceId) VALUES (@UserId, @CmsResourceId)
GO
PRINT N'Creating [dbo].[UserCmsResource_SelectByUser]...';


GO
CREATE PROCEDURE [dbo].[UserCmsResource_SelectByUser]
	@UserId int
AS
	SELECT * FROM [UserCmsResource] WHERE UserId=@UserId
GO
PRINT N'Creating [dbo].[Video_Delete]...';


GO
CREATE PROCEDURE [dbo].[Video_Delete]
@VideoId varchar(50)
AS
	DELETE FROM [dbo].[Video]
	WHERE [VideoId]=@VideoId
GO
PRINT N'Creating [dbo].[Video_Insert]...';


GO
CREATE PROCEDURE [dbo].[Video_Insert]
@VideoId varchar(50),
@AlbumId int,
/*@ImageId VARCHAR(30), */
@CreationDate DATETIME,
@Title       NVARCHAR (MAX),
@Description NVARCHAR (MAX)
/*,@SortOrder   INT*/
AS
	INSERT INTO [dbo].[Video] ([VideoId], [AlbumId], [CreationDate], [Title], [Description])
	VALUES (@VideoId, @AlbumId, @CreationDate, @Title, @Description)
GO
PRINT N'Creating [dbo].[Video_Select]...';


GO
CREATE PROCEDURE [dbo].[Video_Select]
AS
	SELECT [VideoId], [AlbumId], [ImageId], [CreationDate], [Title], [Description], [SortOrder]
	FROM [dbo].[Video]
GO
PRINT N'Creating [dbo].[Video_SelectByAlbum]...';


GO
CREATE PROCEDURE [dbo].[Video_SelectByAlbum]
@AlbumId int
AS
	SELECT [VideoId], [AlbumId], [ImageId], [CreationDate], [Title], [Description], [SortOrder]
	FROM [dbo].[Video]
	WHERE [AlbumId]=@AlbumId
GO
PRINT N'Creating [dbo].[Video_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[Video_SelectOne]
@VideoId varchar(50)
AS
	SELECT [VideoId], [AlbumId], [ImageId], [CreationDate], [Title], [Description], [SortOrder]
	FROM [dbo].[Video]
	WHERE [VideoId]=@VideoId
GO
PRINT N'Creating [dbo].[Video_Update]...';


GO
CREATE PROCEDURE [dbo].[Video_Update]
@VideoId varchar(50),
@AlbumId int,
@ImageId VARCHAR(30),
@CreationDate DATETIME,
@Title       NVARCHAR (MAX),
@Description NVARCHAR (MAX),
@SortOrder   INT
AS
	UPDATE [dbo].[Video]
	SET [AlbumId]=@AlbumId,
		[ImageId]=@ImageId, 
		[CreationDate]=@CreationDate,
		[Title]=@Title, 
		[Description]=@Description, 
		[SortOrder]=@SortOrder
	WHERE [VideoId]=@VideoId
GO
PRINT N'Creating [dbo].[VideoAlbum_Delete]...';


GO
CREATE PROCEDURE [dbo].[VideoAlbum_Delete]
@AlbumId INT
AS
	DELETE FROM [dbo].[VideoAlbum]
	WHERE [AlbumId]=@AlbumId
GO
PRINT N'Creating [dbo].[VideoAlbum_Insert]...';


GO
CREATE PROCEDURE [dbo].[VideoAlbum_Insert]
@Name  NVARCHAR (50),
@CreationDate DATETIME
/*,@OwnerId INT*/
AS
	INSERT INTO [dbo].[VideoAlbum] ([Name], [CreationDate])
	VALUES (@Name, @CreationDate)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[VideoAlbum_Select]...';


GO
CREATE PROCEDURE [dbo].[VideoAlbum_Select]
AS
	SELECT [AlbumId], [Name], [CreationDate], [OwnerId]
	FROM [dbo].[VideoAlbum]
GO
PRINT N'Creating [dbo].[VideoAlbum_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[VideoAlbum_SelectOne]
@AlbumId INT
AS
	SELECT [AlbumId], [Name], [CreationDate], [OwnerId]
	FROM [dbo].[VideoAlbum]
	WHERE [AlbumId] = @AlbumId
GO
PRINT N'Creating [dbo].[VideoAlbum_Update]...';


GO
CREATE PROCEDURE [dbo].[VideoAlbum_Update]
@AlbumId INT,
@Name  NVARCHAR (50),
@CreationDate DATETIME,
@OwnerId INT
AS
	UPDATE [dbo].[VideoAlbum]
	SET [Name]=@Name, 
		[CreationDate]=@CreationDate, 
		[OwnerId]=@OwnerId
	WHERE [AlbumId] = @AlbumId
GO






PRINT N'Creating [News].[Tag_SelectByNews]...';


GO
CREATE PROCEDURE [News].[Tag_SelectByNews]
	@NewsId int
AS
	SELECT t.[TagId], [CreationDate], [Name] FROM [News].[Tag] t JOIN [News].[NewsItemTag] nit ON t.TagId = nit.TagId
		WHERE nit.NewsItemId = @NewsId
GO
PRINT N'Creating [News].[Tag_SelectOne]...';


GO
CREATE PROCEDURE [News].[Tag_SelectOne]
@TagId int
AS
	SELECT [TagId], [CreationDate], [Name] FROM [News].[Tag]
	WHERE [TagId]=@TagId
GO
PRINT N'Creating [News].[Tag_Select]...';


GO
CREATE PROCEDURE [News].[Tag_Select]
AS
	SELECT [TagId], [CreationDate], [Name] FROM [News].[Tag]
GO
PRINT N'Creating [News].[Tag_Insert]...';


GO
CREATE PROCEDURE [News].[Tag_Insert]
@Name nvarchar(200)
AS
	INSERT INTO [News].[Tag] ([Name])
	VALUES (@Name)
	SELECT @@IDENTITY
GO
PRINT N'Creating [News].[Tag_Delete]...';


GO
CREATE PROCEDURE [News].[Tag_Delete]
@TagId int
AS
	DELETE FROM [News].[Tag]
	WHERE [TagId]=@TagId
GO
PRINT N'Creating [News].[NewsItemTag_SelectOne]...';


GO
CREATE PROCEDURE [News].[NewsItemTag_SelectOne]
@NewsItemId int,
@TagId int
AS
	SELECT [NewsItemId], [TagId] FROM [News].[NewsItemTag]
	WHERE [NewsItemId]=@NewsItemId
		 AND [TagId]=@TagId
GO
PRINT N'Creating [News].[NewsItemTag_Select]...';


GO
CREATE PROCEDURE [News].[NewsItemTag_Select]
AS
	SELECT [NewsItemId], [TagId] FROM [News].[NewsItemTag]
GO
PRINT N'Creating [News].[NewsItemTag_Insert]...';


GO
CREATE PROCEDURE [News].[NewsItemTag_Insert]
@NewsItemId int,
@TagId int
AS
	INSERT INTO [News].[NewsItemTag] ([NewsItemId], [TagId])
	VALUES (@NewsItemId, @TagId)
GO
PRINT N'Creating [News].[NewsItemTag_Delete]...';


GO
CREATE PROCEDURE [News].[NewsItemTag_Delete]
@NewsItemId int,
@TagId int
AS
	DELETE FROM [News].[NewsItemTag]
	WHERE [NewsItemId]=@NewsItemId
		 AND [TagId]=@TagId
GO
PRINT N'Creating [News].[NewsItem_SelectOne]...';


GO
CREATE PROCEDURE [News].[NewsItem_SelectOne]
@NewsId int
AS
	SELECT *
	FROM [News].[NewsItem]
	WHERE [NewsId]=@NewsId
GO
PRINT N'Creating [News].[NewsItem_Select]...';


GO
CREATE PROCEDURE [News].[NewsItem_Select]
AS
	SELECT *
	 FROM [News].[NewsItem]
		--WHERE [PostingDate]<=GETUTCDATE()
		ORDER BY [PostingDate] DESC
GO
PRINT N'Creating [News].[NewsItem_Insert]...';


GO
CREATE PROCEDURE [News].[NewsItem_Insert]
@Title nvarchar(MAX),
@Text nvarchar(MAX),
@PostingDate datetime,
@Description nvarchar(MAX),
@MetaDescription nvarchar(MAX),
@Keywords nvarchar(MAX),
@AuthorId int,
@ImageId varchar(30),
@RelativeUrl nvarchar(300),
@CommentTopicId int,
@RecordType varchar(20),
@Filename NVARCHAR(200),
@VideoId varchar(50),
@BlogId int,
@RelatedNewsItemId int,
@EventDate datetime,
@AdditionalHeaders nvarchar(MAX)
AS
	INSERT INTO [News].[NewsItem] ([Title], [Text], [PostingDate], [Description], [MetaDescription], [Keywords], [AuthorId], [ImageId], [RelativeUrl], [CommentTopicId], [RecordType]
	, [Filename], [VideoId], [BlogId], [RelatedNewsItemId], [EventDate], [AdditionalHeaders])
	VALUES (@Title, @Text, @PostingDate, @Description, @MetaDescription, @Keywords, @AuthorId, @ImageId, @RelativeUrl, 
	@CommentTopicId, @RecordType, @Filename, @VideoId, @BlogId, @RelatedNewsItemId, @EventDate, @AdditionalHeaders)
	SELECT @@IDENTITY
GO
PRINT N'Creating [News].[NewsItem_Delete]...';


GO
CREATE PROCEDURE [News].[NewsItem_Delete]
@NewsId int
AS
	DELETE FROM [News].[NewsItem]
	WHERE [NewsId]=@NewsId
GO
PRINT N'Creating [News].[Blog_Select]...';


GO
CREATE PROCEDURE [News].[Blog_Select]

AS
	SELECT * FROM Blog
GO
PRINT N'Creating [News].[Blog_SelectOne]...';


GO
CREATE PROCEDURE [News].[Blog_SelectOne]
	@BlogId int
AS
	SELECT * FROM Blog WHERE BlogId = @BlogId
GO
PRINT N'Creating [News].[Blog_Delete]...';


GO
CREATE PROCEDURE [News].[Blog_Delete]
	@BlogId int
AS
	DELETE FROM Blog WHERE BlogId = @BlogId
GO
PRINT N'Creating [News].[Blog_Update]...';


GO
CREATE PROCEDURE [News].[Blog_Update]
	@BlogId int,
	@OwnerId int,
	@Title nvarchar(500),
	@Subtitle nvarchar(500),
	@RelativeUrl nvarchar(200)
AS
	UPDATE Blog
	SET Title = @Title,
		OwnerId = @OwnerId,
		Subtitle = @Subtitle,
		RelativeUrl = @RelativeUrl
	WHERE @BlogId = BlogId
GO
PRINT N'Creating [News].[Blog_Insert]...';


GO
CREATE PROCEDURE [News].[Blog_Insert]
	@OwnerId int,
	@Title nvarchar(500),
	@Subtitle nvarchar(500),
	@RelativeUrl nvarchar(200)
AS
	INSERT INTO [News].Blog  ([OwnerId], [Title], [Subtitle], [RelativeUrl])
	VALUES (@OwnerId, @Title, @Subtitle, @RelativeUrl)	
	SELECT @@IDENTITY
GO
PRINT N'Creating [News].[Category_SelectOne]...';


GO
CREATE PROCEDURE [News].[Category_SelectOne]
	@CategoryId int
AS
	SELECT * FROM [News].[Category]
		WHERE CategoryId=@CategoryId
GO
PRINT N'Creating [News].[NewsItem_Category_SelectByCategory]...';


GO
CREATE PROCEDURE [News].[NewsItem_Category_SelectByCategory]
	@CategoryId int
AS
	SELECT NewsItemId FROM [News].[NewsItem_Category]
		WHERE [CategoryId]=@CategoryId
GO
PRINT N'Creating [News].[NewsItem_Category_SelectByNews]...';


GO
CREATE PROCEDURE [News].[NewsItem_Category_SelectByNews]
	@NewsId int
AS
	SELECT CategoryId FROM [News].[NewsItem_Category]
		WHERE NewsItemId=@NewsId
GO
PRINT N'Creating [News].[NewsItem_Category_Insert]...';


GO
CREATE PROCEDURE [News].[NewsItem_Category_Insert]
@NewsItemId int,
@CategoryId int
AS
	INSERT INTO [News].[NewsItem_Category] ([NewsItemId], [CategoryId])
	VALUES (@NewsItemId, @CategoryId)
GO
PRINT N'Creating [News].[NewsItem_Category_Delete]...';


GO
CREATE PROCEDURE [News].[NewsItem_Category_Delete]
@NewsItemId int,
@CategoryId int
AS
	DELETE FROM [News].[NewsItem_Category]
	WHERE [NewsItemId]=@NewsItemId
		 AND [CategoryId]=@CategoryId
GO
PRINT N'Creating [News].[Category_Update]...';


GO
CREATE PROCEDURE [News].[Category_Update]
@Name nvarchar(MAX),
@ParentCategoryId int,
@SortOrder int,
@Hidden bit,
@CategoryId int,
@RelativeUrl nvarchar(300)
AS
	UPDATE [News].[Category] SET
		[Name]=@Name,
		[ParentCategoryId]=@ParentCategoryId,
		[SortOrder]=@SortOrder,
		[Hidden]=@Hidden,
		[RelativeUrl]=@RelativeUrl
	WHERE [CategoryId]=@CategoryId
GO
PRINT N'Creating [News].[Category_Select]...';


GO
CREATE PROCEDURE [News].[Category_Select]
@ParentId int
AS
	SELECT * FROM [News].[Category]
		WHERE @ParentId IS NULL AND [ParentCategoryId] IS NULL 
		OR [ParentCategoryId]=@ParentId
GO
PRINT N'Creating [News].[Category_Insert]...';


GO
CREATE PROCEDURE [News].[Category_Insert]
@Name nvarchar(MAX),
@ParentCategoryId int,
@SortOrder int,
@Hidden bit,
@RelativeUrl nvarchar(300)
AS
	INSERT INTO [News].[Category] ([Name], [ParentCategoryId], [SortOrder], [Hidden],[RelativeUrl])
	VALUES (@Name, @ParentCategoryId, @SortOrder, @Hidden, @RelativeUrl)
	SELECT @@IDENTITY
GO
PRINT N'Creating [News].[Category_Delete]...';


GO
CREATE PROCEDURE [News].[Category_Delete]
@CategoryId int
AS
	DELETE FROM [News].[Category]
	WHERE [CategoryId]=@CategoryId
GO
PRINT N'Creating [News].[Tag_DeleteUnassociated]...';


GO
CREATE PROCEDURE [News].[Tag_DeleteUnassociated]
AS
	DELETE FROM [News].[Tag] WHERE TagId NOT IN (SELECT TagId FROM [News].[NewsItemTag])
GO
PRINT N'Creating [News].[Tag_SelectByPattern]...';


GO
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
GO
PRINT N'Creating [News].[NewsItemTag_SelectTagStats]...';


GO
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
GO
PRINT N'Creating [News].[NewsItem_SelectIds]...';


GO
CREATE PROCEDURE [News].[NewsItem_SelectIds]
	@OnlyPosted bit
AS
	SELECT NewsId FROM [News].[NewsItem]
		WHERE @OnlyPosted=0 OR [PostingDate]<=GETUTCDATE()
		ORDER BY [PostingDate] DESC
GO
PRINT N'Creating [News].[Category_SelectAll]...';


GO
CREATE PROCEDURE [News].[Category_SelectAll]

AS
	SELECT * FROM [News].[Category]
GO
PRINT N'Creating [News].[Blog_SelectByOwner]...';


GO
CREATE PROCEDURE [News].[Blog_SelectByOwner]
	@OwnerId int

AS

SELECT * FROM Blog WHERE OwnerId = @OwnerId
GO
PRINT N'Creating [News].[NewsItem_SelectRelated]...';


GO
CREATE PROCEDURE [News].[NewsItem_SelectRelated]
	@NewsId int,
	@WithSubnews bit,
	@Count int,
	@OnlyPosted bit
AS

DECLARE @FullNewsIds TABLE(Val INT)

IF @WithSubnews = 1
BEGIN
	;WITH ret AS(
    		SELECT	[NewsId], [RelatedNewsItemId]
    		FROM	[NewsItem]
    		WHERE	[RelatedNewsItemId] = @NewsId
    		UNION ALL
    		SELECT	t.[NewsId], t.[RelatedNewsItemId]
    		FROM	[NewsItem] t INNER JOIN
    				ret r ON t.[RelatedNewsItemId] = r.[NewsId]
	)
	INSERT INTO @FullNewsIds
	SELECT NewsId
	FROM ret
END
ELSE BEGIN
	INSERT INTO @FullNewsIds
	SELECT [NewsId] FROM [NewsItem]
	WHERE [RelatedNewsItemId] = @NewsId
END


SELECT TOP(@Count) NewsId FROM [NewsItem] 
WHERE NewsId IN (SELECT * FROM @FullNewsIds) AND (@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE()) 
ORDER BY PostingDate DESC
GO
PRINT N'Creating [News].[Category_SelectByUrl]...';


GO
CREATE PROCEDURE [News].[Category_SelectByUrl]
	@RelativeUrl nvarchar(300)
AS
	SELECT * From Category WHERE RelativeUrl = @RelativeUrl
GO
PRINT N'Creating [News].[Blog_SelectOneByRelativeUrl]...';


GO
CREATE PROCEDURE [News].[Blog_SelectOneByRelativeUrl]
	@RelativeUrl nvarchar(200)
AS
	SELECT * FROM [News].Blog WHERE [RelativeUrl]=@RelativeUrl
GO
PRINT N'Creating [News].[Blog_User_SelectByUser]...';


GO
CREATE PROCEDURE [News].[Blog_User_SelectByUser]
	@UserId int
AS
	SELECT * FROM [News].[Blog_User] WHERE UserId=@UserId
GO
PRINT N'Creating [News].[Blog_User_SelectByBlog]...';


GO
CREATE PROCEDURE [News].[Blog_User_SelectByBlog]
	@BlogId int
AS
	SELECT * FROM [News].[Blog_User] WHERE BlogId=@BlogId
GO
PRINT N'Creating [News].[Blog_User_Delete]...';


GO
CREATE PROCEDURE [News].[Blog_User_Delete]
	@BlogId int,
	@UserId int
AS
	DELETE FROM Blog_User
	WHERE BlogId = @BlogId AND UserId = @UserId
GO
PRINT N'Creating [News].[Blog_User_Insert]...';


GO
CREATE PROCEDURE [News].[Blog_User_Insert]
	@BlogId int,
	@UserId int
AS
	INSERT INTO [Blog_User] ([BlogId],[UserId])
	VALUES (@BlogId, @UserId)
GO
PRINT N'Creating [News].[NewsItem_SelectIdsByTagName]...';


GO
CREATE PROCEDURE [News].[NewsItem_SelectIdsByTagName]
	@TagName NVARCHAR(200),
	@OnlyPosted bit
AS
		SELECT ni.NewsId
		FROM [News].[NewsItem] ni join [News].[NewsItemTag] nit ON ni.NewsId= nit.NewsItemId
			join [News].[Tag] t ON  nit.TagId = t.TagId
		WHERE t.Name = @TagName AND
			(@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE())
		ORDER BY [PostingDate] DESC
GO
PRINT N'Creating [News].[NewsItem_SelectFilteredPage]...';


GO
CREATE PROCEDURE [News].[NewsItem_SelectFilteredPage]
	@NewsIds Int_Table READONLY,
	@PageNumber int,
	@PageSize int,
	@TotalCount int OUT,
	@OnlyFutureEventDate bit,
	@OnlyPosted bit,
	@BlogId int,
	@RecordTypes String_Table READONLY,

	@SortBy VARCHAR(20),
	@SortOrder VARCHAR(4)

AS

	DECLARE @news TABLE(
	[NewsId]       INT             NOT NULL,
	[Title]        NVARCHAR (MAX) NOT NULL,
	[Text]         NVARCHAR (MAX) NOT NULL,
	[PostingDate]  DATETIME       NOT NULL,
	[Description]  NVARCHAR (MAX) NOT NULL,
	[MetaDescription]  NVARCHAR (MAX) NULL,
	[Keywords]     NVARCHAR (MAX) NULL,
	[CreationDate] DATETIME       NOT NULL,
	[AuthorId]     INT            NOT NULL,
	[ImageId]      VARCHAR (30)   NULL,
	[RelativeUrl]  NVARCHAR (300)  NOT NULL,
	[CommentTopicId] INT NULL,	
	[RecordType] VARCHAR(20) NULL,
	[Filename] NVARCHAR(200) NULL,
	[VideoId] varchar(50) NULL,
	[BlogId] INT NULL,
	[EventDate]  DATETIME NULL,
	[AdditionalHeaders] NVARCHAR (MAX) NULL
	)
	
		INSERT INTO @news ([NewsId], [Title], [Text], [PostingDate], [Description], [MetaDescription], [Keywords], [CreationDate], [AuthorId], [ImageId], [RelativeUrl], [CommentTopicId],[RecordType], [Filename], [VideoId], [BlogId], [EventDate], [AdditionalHeaders] )
	SELECT DISTINCT ni.[NewsId], [Title], [Text], [PostingDate], [Description], [MetaDescription], [Keywords], ni.[CreationDate], [AuthorId], [ImageId], [RelativeUrl], [CommentTopicId] ,[RecordType], [Filename], [VideoId], [BlogId], [EventDate], [AdditionalHeaders]
		FROM [News].[NewsItem] ni 
			JOIN @NewsIds ids ON ni.NewsId=ids.Val
		WHERE 
		
		(@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE())
		AND
		(
			@OnlyFutureEventDate IS NULL
			OR
			@OnlyFutureEventDate=0 AND [EventDate]<GETUTCDATE()
			OR
			@OnlyFutureEventDate=1 AND [EventDate]>=GETUTCDATE()
		)
		AND
		(
			NOT EXISTS(SELECT * FROM @RecordTypes)
				OR
			[RecordType] IN (SELECT * FROM @RecordTypes)
		)
		AND
		(@BlogId IS NULL OR [BlogId]=@BlogId)

	SELECT @TotalCount = COUNT(*) FROM @news

IF @SortBy='PostingDate'
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
ELSE
IF @SortBy='CreationDate'
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [CreationDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [CreationDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
ELSE
IF @SortBy='EventDate'
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [EventDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [EventDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
ELSE
BEGIN
	IF @SortOrder = 'Asc'
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] ASC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
	ELSE
	BEGIN
		SELECT * FROM @news
		ORDER BY [PostingDate] DESC
		OFFSET (@PageNumber - 1 )*@PageSize ROWS
		FETCH NEXT @PageSize ROWS ONLY
	END
END
GO
PRINT N'Creating [News].[NewsItem_Update]...';


GO
CREATE PROCEDURE [News].[NewsItem_Update]
@Title nvarchar(MAX),
@Text nvarchar(MAX),
@PostingDate datetime,
@Description nvarchar(MAX),
@MetaDescription nvarchar(MAX),
@Keywords nvarchar(MAX),
@ImageId varchar(30),
@RelativeUrl nvarchar(300),
@CommentTopicId int,
@RecordType varchar(20),
@Filename NVARCHAR(200),
@VideoId varchar(50),
@NewsId int,
@RelatedNewsItemId int,
@EventDate datetime,
@AdditionalHeaders nvarchar(MAX)
AS
	UPDATE [News].[NewsItem] SET
		[Title]=@Title,
		[Text]=@Text,
		[PostingDate]=@PostingDate,
		[Description]=@Description,
		[MetaDescription]=@MetaDescription,
		[Keywords]=@Keywords,
		[ImageId]=@ImageId,
		[RelativeUrl]=@RelativeUrl,
		[CommentTopicId]=@CommentTopicId,
		[RecordType]=@RecordType,
		[Filename]=@Filename,
		[VideoId]=@VideoId,
		[RelatedNewsItemId]=@RelatedNewsItemId,
		[EventDate]=@EventDate,
		[AdditionalHeaders]=@AdditionalHeaders
	WHERE [NewsId]=@NewsId
GO
PRINT N'Creating [News].[NewsItem_SelectByUrl]...';


GO
CREATE PROCEDURE [News].[NewsItem_SelectByUrl]
	@RelativeUrl nvarchar(300),
	@OnlyPosted bit
AS
		SELECT *
		FROM [News].[NewsItem]
	WHERE [RelativeUrl]=@RelativeUrl AND 
		(@OnlyPosted=0 OR [PostingDate]<=GETUTCDATE())
GO
PRINT N'Creating [Comments].[Comment_Delete]...';


GO
CREATE PROCEDURE [Comments].[Comment_Delete]
	@CommentId int
AS
	--DELETE FROM [Comments].[Comment] WHERE [CommentId]=@CommentId
	UPDATE [Comments].[Comment] SET [Deleted]=1 WHERE [CommentId]=@CommentId
GO
PRINT N'Creating [Comments].[Comment_Insert]...';


GO
CREATE PROCEDURE [Comments].[Comment_Insert]
	@ParentCommentId int,
	@CommentTopicId int,
	@Text NVARCHAR(MAX),
	@Moderated BIT,
	@AuthorId INT,
	@Url nvarchar(200),
	@Name nvarchar(200),
	@Email nvarchar(200)
AS
	INSERT INTO [Comments].[Comment] VALUES(@ParentCommentId, @CommentTopicId,
		@Text, @Moderated, @AuthorId, GETUTCDATE(), 0, @Url, @Email, @Name)
	SELECT @@IDENTITY
GO
PRINT N'Creating [Comments].[Comment_SelectByAuthor]...';


GO
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
GO
PRINT N'Creating [Comments].[Comment_SelectByTopic]...';


GO
CREATE PROCEDURE [Comments].[Comment_SelectByTopic]
	@CommentTopicId int,
	@Moderated BIT = NULL
AS
	IF @Moderated IS NULL
		SELECT * FROM [Comments].[Comment] WHERE [CommentTopicId]=@CommentTopicId
			ORDER BY [CreationDate] DESC
	ELSE
		SELECT * FROM [Comments].[Comment] WHERE [CommentTopicId]=@CommentTopicId AND [Moderated]=@Moderated
			ORDER BY [CreationDate] DESC
GO
PRINT N'Creating [Comments].[Comment_SelectOne]...';


GO
CREATE PROCEDURE [Comments].[Comment_SelectOne]
	@CommentId INT
AS
	SELECT * FROM [Comments].[Comment] WHERE [CommentId] = @CommentId
GO
PRINT N'Creating [Comments].[Comment_UpdateModerated]...';


GO
CREATE PROCEDURE [Comments].[Comment_UpdateModerated]
	@CommentId int,
	@Moderated BIT
AS
	UPDATE [Comments].[Comment] SET [Moderated]=@Moderated WHERE [CommentId]=@CommentId
GO
PRINT N'Creating [Comments].[Comment_UpdateText]...';


GO
CREATE PROCEDURE [Comments].[Comment_UpdateText]
	@CommentId int,
	@Text NVARCHAR(MAX)
AS
	UPDATE [Comments].[Comment] SET [Text]=@Text WHERE [CommentId]=@CommentId
GO
PRINT N'Creating [Comments].[CommentTopic_Delete]...';


GO
CREATE PROCEDURE [Comments].[CommentTopic_Delete]
	@CommentTopicId int
AS
	DELETE FROM [Comments].[CommentTopic] WHERE [CommentTopicId]=@CommentTopicId
GO
PRINT N'Creating [Comments].[CommentTopic_Insert]...';


GO
CREATE PROCEDURE [Comments].[CommentTopic_Insert]
	@TargetType VARCHAR(50),
	@TargetId INT,
	@TargetUrl NVARCHAR(2000),
	@TargetTitle NVARCHAR(300)
AS
	INSERT INTO [Comments].[CommentTopic] VALUES (@TargetType, @TargetId, @TargetUrl, @TargetTitle)
	SELECT @@IDENTITY
GO
PRINT N'Creating [Comments].[CommentTopic_Select]...';


GO
CREATE PROCEDURE [Comments].[CommentTopic_Select]
	@Page int,
	@PageSize int,
	@TotalCount int out,
	@TopicType VARCHAR(50) = null

AS
	DECLARE @temp TABLE (
		[CommentTopicId] INT,
		[TargetType] VARCHAR(50),
		[TargetId] INT,
		[TargetUrl] NVARCHAR(2000),
		[TargetTitle] NVARCHAR(300)
		)
	INSERT INTO @temp ([CommentTopicId], [TargetType], [TargetId], [TargetUrl], [TargetTitle])
		(SELECT [CommentTopicId], [TargetType], [TargetId], [TargetUrl], [TargetTitle] FROM [Comments].[CommentTopic])

	SELECT @TotalCount = COUNT(*) FROM @temp
	
	SELECT * FROM @temp
	ORDER BY [CommentTopicId] DESC
	OFFSET (@Page - 1 )*@PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY
GO
PRINT N'Creating [Comments].[CommentTopic_SelectCommentCount]...';


GO
CREATE PROCEDURE [Comments].[CommentTopic_SelectCommentCount]
	@CommentTopicId int
AS
	SELECT COUNT(*) FROM [Comments].[Comment] WHERE [CommentTopicId]=@CommentTopicId
GO
PRINT N'Creating [Comments].[CommentTopic_SelectOne]...';


GO
CREATE PROCEDURE [Comments].[CommentTopic_SelectOne]
	@CommentTopicId int
AS
	SELECT * FROM [Comments].[CommentTopic] WHERE [CommentTopicId]=@CommentTopicId
GO
GO
PRINT N'Creating [dbo].[OrderForm_Select]...';


GO
CREATE PROCEDURE [dbo].[OrderForm_Select]
AS
	SELECT * FROM [dbo].[OrderForm]
GO
PRINT N'Creating [dbo].[OrderForm_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[OrderForm_SelectOne]
@OrderFormId int
AS
	SELECT * FROM [dbo].[OrderForm]
	WHERE [OrderFormId]=@OrderFormId
GO
PRINT N'Creating [dbo].[OrderForm_Delete]...';


GO
CREATE PROCEDURE [dbo].[OrderForm_Delete]
@OrderFormId int
AS
	DELETE FROM [dbo].[OrderForm]
	WHERE [OrderFormId]=@OrderFormId
GO
PRINT N'Creating [dbo].[OrderFormField_SelectOne]...';


GO
CREATE PROCEDURE [dbo].[OrderFormField_SelectOne]
@OrderFormFieldId int
AS
	SELECT * FROM [dbo].[OrderFormField]
	WHERE [OrderFormFieldId]=@OrderFormFieldId
GO
PRINT N'Creating [dbo].[OrderFormField_SelectByForm]...';


GO
CREATE PROCEDURE [dbo].[OrderFormField_SelectByForm]
	@OrderFormId int
AS
	SELECT * FROM [dbo].[OrderFormField]
	WHERE OrderFormId = @OrderFormId
GO
PRINT N'Creating [dbo].[OrderFormField_Delete]...';


GO
CREATE PROCEDURE [dbo].[OrderFormField_Delete]
@OrderFormFieldId int
AS
	DELETE FROM [dbo].[OrderFormField]
	WHERE [OrderFormFieldId]=@OrderFormFieldId
GO
PRINT N'Creating [dbo].[OrderFormField_Update]...';


GO
CREATE PROCEDURE [dbo].[OrderFormField_Update]
@LabelText nvarchar(100),
@ValueType varchar(20),
@Required bit,
@OrderFormId int,
@OrderFormFieldId int,
@SortOrder int
AS
	UPDATE [dbo].[OrderFormField] SET
		[LabelText]=@LabelText,
		[ValueType]=@ValueType,
		[Required]=@Required,
		[OrderFormId]=@OrderFormId,
		[SortOrder]=@SortOrder
	WHERE [OrderFormFieldId]=@OrderFormFieldId
GO
PRINT N'Creating [dbo].[OrderFormField_Insert]...';


GO
CREATE PROCEDURE [dbo].[OrderFormField_Insert]
@LabelText nvarchar(100),
@ValueType varchar(20),
@Required bit,
@OrderFormId int,
@SortOrder int
AS
	INSERT INTO [dbo].[OrderFormField] ([LabelText], [ValueType], [Required], [OrderFormId], [SortOrder])
	VALUES (@LabelText, @ValueType, @Required, @OrderFormId, @SortOrder)
	SELECT @@IDENTITY
GO
PRINT N'Creating [dbo].[OrderForm_Update]...';


GO
CREATE PROCEDURE [dbo].[OrderForm_Update]
@EmailSubject nvarchar(300),
@DateInEmailSubject bit,
@Email nvarchar(100),
@BccEmail nvarchar(100),
@HtmlTemplate nvarchar(MAX),
@RedirectUrl nvarchar(500),
@SuccessMessage nvarchar(500),
@MetricsCode nvarchar(50),
@FileAttachmentEnabled bit,
@MaxFileAttachmentsCount int,
@OrderFormId int,
@Title NVARCHAR(200),
@EmailTemplate nvarchar(MAX)
AS
	UPDATE [dbo].[OrderForm] SET
		[EmailSubject]=@EmailSubject,
		[DateInEmailSubject]=@DateInEmailSubject,
		[Email]=@Email,
		[BccEmail]=@BccEmail,
		[HtmlTemplate]=@HtmlTemplate,
		[RedirectUrl]=@RedirectUrl,
		[SuccessMessage]=@SuccessMessage,
		[MetricsCode]=@MetricsCode,
		[FileAttachmentEnabled]=@FileAttachmentEnabled,
		[MaxFileAttachmentsCount]=@MaxFileAttachmentsCount,
		[Title]=@Title,
		[EmailTemplate]=@EmailTemplate
	WHERE [OrderFormId]=@OrderFormId
GO
PRINT N'Creating [dbo].[OrderForm_Insert]...';


GO
CREATE PROCEDURE [dbo].[OrderForm_Insert]
@EmailSubject nvarchar(300),
@DateInEmailSubject bit,
@Email nvarchar(100),
@BccEmail nvarchar(100),
@HtmlTemplate nvarchar(MAX),
@RedirectUrl nvarchar(500),
@SuccessMessage nvarchar(500),
@MetricsCode nvarchar(50),
@FileAttachmentEnabled bit,
@MaxFileAttachmentsCount int,
@Title NVARCHAR(200),
@EmailTemplate nvarchar(MAX)
AS
	INSERT INTO [dbo].[OrderForm] ([EmailSubject], [DateInEmailSubject], [Email], [BccEmail], [HtmlTemplate], [RedirectUrl], [SuccessMessage], [MetricsCode], [FileAttachmentEnabled], [MaxFileAttachmentsCount], [Title], [EmailTemplate])
	VALUES (@EmailSubject, @DateInEmailSubject, @Email, @BccEmail, @HtmlTemplate, @RedirectUrl, @SuccessMessage, @MetricsCode, @FileAttachmentEnabled, @MaxFileAttachmentsCount, @Title, @EmailTemplate)
	SELECT @@IDENTITY
GO

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '32c84dab-cd5e-484d-b8cc-7249db0a1979')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('32c84dab-cd5e-484d-b8cc-7249db0a1979')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '448b1f51-edde-4e26-8af6-4c730c4a06c4')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('448b1f51-edde-4e26-8af6-4c730c4a06c4')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '81772b9f-4ab3-4667-baeb-19cbac23b9db')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('81772b9f-4ab3-4667-baeb-19cbac23b9db')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '8da056fa-c952-4afa-a87c-9795a92efc81')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('8da056fa-c952-4afa-a87c-9795a92efc81')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '2e189d63-fc20-4f5f-8311-50580d2b4fea')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('2e189d63-fc20-4f5f-8311-50580d2b4fea')

GO

/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Если нет ресурсов
IF (SELECT COUNT(*) FROM [dbo].[CmsResource]) = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[CmsResource] ON
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (1, N'Users', N'Пользователи')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (2, N'News', N'Новости')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (3, N'Gallery', N'Галерея')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (4, N'Albums', N'Альбомы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (5, N'CommonSettings', N'Настройка')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (6, N'Menus', N'Меню')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (7, N'Pages', N'Страницы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (8, N'Blocks', N'Блоки')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (9, N'Sliders', N'Слайдеры')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (10, N'Shop', N'Магазин')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (11, N'Reviews', N'Отзывы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (13, N'Analytics', N'Аналитика')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (15, N'AdminPanel', N'Админка')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (16, N'Shop_Actions', N'Магазин: Акции')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (17, N'Shop_Orders', N'Магазин: Заказы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (18, N'Shop_Packs', N'Магазин: Упаковки')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (19, N'Shop_Clients', N'Магазин: Клиенты')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (20, N'Shop_Reviews', N'Магазин: Отзывы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (21, N'Shop_YmlExport', N'Магазин: Экспорт YML')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (22, N'Shop_Settings', N'Магазин: Настройки')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (23, N'Shop_PickUpPoints', N'Магазин: Пункты самовывоза')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (24, N'Shop_RegularClients', N'Магазин: Постоянным клиентам')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (25, N'DeleteObjects', N'Удаление объектов')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (26, N'SupportTickets', N'Поддержка')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (27, N'Shop_Currencies', N'Магазин: Валюты')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (28, N'Shop_OrderForms', N'Магазин: Формы заказа')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (100, N'Shop_MassPriceChange', N'Магазин: Пакетные изменения цен')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (34, N'CommentsEditor', N'Комментарии')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (40, N'Emails', N'Письма')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (50, N'UploadFiles', N'Файлы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (51, N'Requests', N'База заявок')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (52, N'FAQ', N'Вопросы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (60, N'Development', N'Разработка')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (61, N'Dev_CodeEditor', N'Разработка: Редактор кода')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (62, N'Dev_Widgets', N'Разработка: Виджеты')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (63, N'Dev_InterfaceStrings', N'Разработка: Строки интерфейса')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (64, N'Dev_Database', N'Разработка: База данных')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (65, N'RedirectToPageRoutes', N'Редиректы')
	INSERT INTO [dbo].[CmsResource] ([CmsResourceId], [Name], [Description]) VALUES (70, N'Dev_MagicButton', N'Разработка: Кнопка входа')
	
	SET IDENTITY_INSERT [dbo].[CmsResource] OFF
END
GO


-- Если нет юзверя admin
IF (SELECT COUNT(*) FROM [dbo].[User] WHERE [Username]='admin') = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[User] ON
	-- Login/Password: admin/admin
	INSERT INTO [dbo].[User] ([UserId], [Username], [Password]) VALUES (1, N'admin', N'21232f297a57a5a743894a0e4a801fc3')
	SET IDENTITY_INSERT [dbo].[User] OFF

	-- Выдача всех прав админу
	INSERT INTO [dbo].[UserCmsResource]
	SELECT [CmsResourceId], 1
	FROM [dbo].[CmsResource]
END
GO

-- Если нет страниц
IF (SELECT COUNT(*) FROM [dbo].[Page]) = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[Page] ON
	INSERT INTO [dbo].[Page] ([Title], [Annotation], [Content], [CreationDate], [RelativeUrl], [Keywords], [PageId]) VALUES (N'Главная', N'Главная страница', N'<p>Главная страница</p>
	', GETUTCDATE(), N'Главная', NULL, 1)
	SET IDENTITY_INSERT [dbo].[Page] OFF
END
GO

-- Если нет блоков
IF (SELECT COUNT(*) FROM [dbo].[Block]) = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[Block] ON
	INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (1, N'Шапка', N'<p>Шапка</p>')
	INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (2, N'Подвал', N'<p>Подвал</p>')
	INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (3, N'Метрики', N'<p></p>')
	INSERT INTO [dbo].[Block] ([BlockId], [Title], [Content]) VALUES (4, N'Копирайт', N'<p>&copy;</p>')
	SET IDENTITY_INSERT [dbo].[Block] OFF
END
GO

-- Если нет меню
IF (SELECT COUNT(*) FROM [dbo].Menu) = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[Menu] ON
	INSERT INTO [dbo].[Menu] ([MenuId], [Name]) VALUES (1, N'Главное')
	SET IDENTITY_INSERT [dbo].[Menu] OFF
END
GO

-- Если нет элементов меню
IF (SELECT COUNT(*) FROM [dbo].MenuItem) = 0
BEGIN
	SET IDENTITY_INSERT [dbo].[MenuItem] ON
	INSERT INTO [dbo].[MenuItem] ([MenuItemId], [Name], [MenuId], [ParentMenuItemId], [PageUrl], [SortOrder]) VALUES (1, N'Главная', 1, NULL, N'Главная', 0)
	SET IDENTITY_INSERT [dbo].[MenuItem] OFF
END
GO

-- Если нет настроек
IF (SELECT COUNT(*) FROM [dbo].Setting) = 0
BEGIN
	--SET IDENTITY_INSERT [dbo].[Setting] ON
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MainMenuId', N'1')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MainPageUrl', N'Главная')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SiteName', N'Новый сайт')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'Reviews', N'False')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'TicketLifetime', N'02:00:00')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'CommentsPremoderation', N'False')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'Timezone', 3)
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplOrderAutoReply', N'<p>Здравствуйте, {0}!<br /><br />Мы получили Ваше обращение и свяжемся с Вами.<br /><br />Данные, которые Вы нам отправили:<br /><br />{1}</p>')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplOrder', N'<p><strong>Имя клиента: </strong>{0}</p>
<p><strong>Email: </strong>
<a href="mailto:{1}">{1}
</a>
</p>
<p><strong>Телефон: </strong>{2}</p>
<p>{4}</p>')
		INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplCallback', N'<h1>Обратный звонок</h1>
<p><strong>Имя клиента: </strong>{0}</p>
<p><strong>Телефон: </strong>{1}</p>
<p><strong>Удобное время: </strong>{2}</p>')
		INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'MailTmplReviewCreated', N'<h1>Новый отзыв</h1>
<p><strong>Имя: </strong>{0}</p>
<p><strong>Email: </strong>{1}</p>
<p><strong>Текст отзыва:</strong></p>
<p>{2}</p>')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'RootUrl', N'http://localhost:4014')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ImageMaxHeight', 1080)
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ImageMaxWidth', 1920)
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ImageQuality', 90)
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ThumbnailHeight', 350)
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ThumbnailWidth', 300)
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'AutoEmailReplyEnabled', N'True')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailSmtpUrl', N'smtp.yandex.ru')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailSmtpPort', 587)
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SmtpSslEnabled', N'True')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailLogin', N'mail1@yandex.ru')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'EmailPassword', N'mailPassword')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'OrderEmailAddress', N'mail2@yandex.ru')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SystemEmailAddress', N'mail1@yandex.ru')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'SystemEmailSenderName', N'Автоматическое сообщение')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'TranslitEnabled', N'True')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'AllowedFileExtensions', N'.doc,.docx,.xls,.xlsx,.pdf,.txt')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'YoutubeAPIKey', N'AIzaSyCQgKGPuIQwKvZvFLHjZ_sjr3ZB8ijQ4rA')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ThumbnailSizes', N'400w,200h')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ReviewCreatedNotification', N'False')
	INSERT INTO [dbo].[Setting] ([Key], [Value]) VALUES (N'ReviewSort', N'CreationDateDesc')
	--SET IDENTITY_INSERT [dbo].[Setting] OFF
END
GO

-- Если нет стран
IF (SELECT COUNT(*) FROM [dbo].Country) = 0
BEGIN
	INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (1, N'Россия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (2, N'Австралия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (3, N'Австрия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (4, N'Азербайджан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (5, N'Албания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (6, N'Алжир')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (7, N'Ангилья (Ангуилла)')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (8, N'Ангола')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (9, N'Андорра')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (10, N'Антигуа и Барбуда')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (11, N'Аргентина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (12, N'Армения')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (13, N'Аруба')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (14, N'Афганистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (15, N'Багамские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (16, N'Бангладеш')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (17, N'Барбадос')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (18, N'Бахрейн')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (19, N'Беларусь')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (20, N'Белиз')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (21, N'Бельгия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (22, N'Бенин')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (23, N'Бермудские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (24, N'Болгария')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (25, N'Боливия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (26, N'Босния и Герцеговина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (27, N'Ботсвана')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (28, N'Бразилия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (29, N'Британские Виргинские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (30, N'Бруней')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (31, N'Буркина-Фасо')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (32, N'Бурунди')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (33, N'Бутан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (34, N'Вануату')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (35, N'Ватикан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (36, N'Великобритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (37, N'Венгрия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (38, N'Венесуэла')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (39, N'Виргинские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (40, N'Восточное (Американское) Самоа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (41, N'Восточный Тимор')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (42, N'Вьетнам')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (43, N'Габон')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (44, N'Гаити')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (45, N'Гайана')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (46, N'Гамбия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (47, N'Гана')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (48, N'Гваделупа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (49, N'Гватемала')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (50, N'Гвинея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (51, N'Гвинея-Бисау')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (52, N'Германия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (53, N'Гибралтар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (54, N'Гондурас')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (55, N'Гренада')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (56, N'Гренландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (57, N'Греция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (58, N'Грузия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (59, N'Гуам')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (60, N'Дания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (61, N'Демократическая Республика Конго')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (62, N'Джибути')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (63, N'Доминика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (64, N'Доминиканская республика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (65, N'Египет')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (66, N'Замбия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (67, N'Западная Сахара')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (68, N'Западное Самоа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (69, N'Зимбабве')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (70, N'Израиль')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (71, N'Индия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (72, N'Индонезия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (73, N'Иордания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (74, N'Ирак')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (75, N'Иран')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (76, N'Ирландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (77, N'Исландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (78, N'Испания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (79, N'Италия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (80, N'Йемен')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (81, N'Кабо-Верде')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (82, N'Казахстан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (83, N'Каймановы острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (84, N'Камбоджа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (85, N'Камерун')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (86, N'Канада')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (87, N'Катар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (88, N'Кения')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (89, N'Кипр')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (90, N'Киргизия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (91, N'Кирибати')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (92, N'Китай')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (93, N'КНДР')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (94, N'Кокосовые острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (95, N'Колумбия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (96, N'Коморские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (97, N'Коста-Рика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (98, N'Кот-д`Ивуар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (99, N'Куба')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (100, N'Кувейт')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (101, N'Кука острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (102, N'Лаос')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (103, N'Латвия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (104, N'Лесото')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (105, N'Либерия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (106, N'Ливан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (107, N'Ливия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (108, N'Литва')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (109, N'Лихтенштейн')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (110, N'Люксембург')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (111, N'Маврикий')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (112, N'Мавритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (113, N'Мадагаскар')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (114, N'Макао (Аомынь)')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (115, N'Македония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (116, N'Малави')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (117, N'Малайзия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (118, N'Мали')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (119, N'Мальдивы')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (120, N'Мальта')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (121, N'Марокко')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (122, N'Мартиника')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (123, N'Маршаловы острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (124, N'Мексика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (125, N'Мидуэй')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (126, N'Микронезия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (127, N'Мозамбик')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (128, N'Молдавия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (129, N'Монако')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (130, N'Монголия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (131, N'Монтсеррат')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (132, N'Мьянма ( Бирма )')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (133, N'Намибия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (134, N'Народная Республика Конго')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (135, N'Науру')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (136, N'Непал')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (137, N'Нигер')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (138, N'Нигерия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (139, N'Нидерландские Антиллы')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (140, N'Нидерланды')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (141, N'Никарагуа')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (142, N'Ниуэ')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (143, N'Новая Зеландия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (144, N'Новая Каледония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (145, N'Норвегия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (146, N'Норфолк')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (147, N'ОАЭ')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (148, N'Оман')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (149, N'Пакистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (150, N'Палау')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (151, N'Палестина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (152, N'Панама')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (153, N'Папуа-Новая Гвинея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (154, N'Парагвай')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (155, N'Перу')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (156, N'Питкэрн')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (157, N'Польша')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (158, N'Португалия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (159, N'Пуэрто-Рико')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (160, N'Реюньон')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (161, N'Рождества остров')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (162, N'Руанда')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (163, N'Румыния')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (164, N'Сальвадор')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (165, N'Сан-Марино')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (166, N'Сан-Томе и Принсипи')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (167, N'Саудовская Аравия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (168, N'Свазиленд')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (169, N'Святой Елены Остров')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (170, N'Северные Марианские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (171, N'Сейшельские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (172, N'Сенегал')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (173, N'Сен-Пьер и Микелон')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (174, N'Сент-Винсент и Гренадины')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (175, N'Сент-Китс и Невис')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (176, N'Сент-Люсия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (177, N'Сербия и Черногория')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (178, N'Сингапур')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (179, N'Сирия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (180, N'Словакия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (181, N'Словения')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (182, N'Сомали')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (183, N'Судан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (184, N'Суринам')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (185, N'США')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (186, N'Сьерра-Леоне')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (187, N'Таджикистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (188, N'Таиланд')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (189, N'Танзания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (190, N'Тёркс и Кайкос')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (191, N'Того')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (192, N'Токелау')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (193, N'Тонга')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (194, N'Тринидад и Тобаго')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (195, N'Тувалу')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (196, N'Тунис')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (197, N'Туркменистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (198, N'Турция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (199, N'Уганда')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (200, N'Узбекистан')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (201, N'Украина')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (202, N'Уоллис и Футуна')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (203, N'Уругвай')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (204, N'Уэйк')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (205, N'Фарерские острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (206, N'Фиджи')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (207, N'Филиппины')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (208, N'Финляндия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (209, N'Фолклендские (Мальвинские) острова')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (210, N'Франция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (211, N'Французская полинезия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (212, N'Хорватия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (213, N'Центрально-Африканская республика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (214, N'ЧАД')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (215, N'Черногория')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (216, N'Чехия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (217, N'Чили')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (218, N'Швейцария')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (219, N'Швеция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (220, N'Шри-Ланка')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (221, N'Эквадор')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (222, N'Экваториальная Гвинея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (223, N'Эритрея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (224, N'Эстония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (225, N'Эфиопия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (226, N'Южная Корея')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (227, N'Южно-Африканская Республика')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (228, N'Ямайка')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (229, N'Япония')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (230, N'«Гвиана» Франция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (231, N'«Майотта» Франция')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (232, N'«Парасельские острова» КНР, Вьетнам, Тайвань')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (233, N'«Сеута и Мелилья» Испания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (234, N'«Спартли острова» КНР, Вьетнам, Тайвань, Малайзия, Филиппины, Бруней')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (235, N'«Чагос острова» Великобритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (236, N'«Шпицберген» Норвегия')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (237, N'«Южная Георгия» Великобритания')
INSERT INTO [dbo].[Country] ([CountryId], [Name]) VALUES (238, N'«Южные Сандвичевы острова» Фолклендские острова')
END
GO

/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Если нет блогов
IF NOT EXISTS (SELECT * FROM [News].[Blog])
BEGIN
	DECLARE @adminId INT

	SELECT @adminId=UserId FROM [dbo].[User] WHERE Username='admin'

	SET IDENTITY_INSERT [News].[Blog] ON
	INSERT INTO [News].[Blog] ([BlogId], [Title], [OwnerId]) VALUES 
		(1, N'Новости сайта', @adminId)
	INSERT INTO [News].[Blog_User] ([BlogId],[UserId]) VALUES (1, @adminId) 
	SET IDENTITY_INSERT [News].[Blog] OFF
END
GO

GO
DECLARE @VarDecimalSupported AS BIT;

SELECT @VarDecimalSupported = 0;

IF ((ServerProperty(N'EngineEdition') = 3)
    AND (((@@microsoftversion / power(2, 24) = 9)
          AND (@@microsoftversion & 0xffff >= 3024))
         OR ((@@microsoftversion / power(2, 24) = 10)
             AND (@@microsoftversion & 0xffff >= 1600))))
    SELECT @VarDecimalSupported = 1;

IF (@VarDecimalSupported > 0)
    BEGIN
        EXECUTE sp_db_vardecimal_storage_format N'$(DatabaseName)', 'ON';
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET MULTI_USER 
    WITH ROLLBACK IMMEDIATE;


GO
PRINT N'Update complete.';


GO
