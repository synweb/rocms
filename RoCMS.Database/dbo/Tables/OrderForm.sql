CREATE TABLE [dbo].[OrderForm]
(
	[OrderFormId] INT NOT NULL PRIMARY KEY IDENTITY,
	[EmailSubject] nvarchar(300),
	[DateInEmailSubject] bit NOT NULL DEFAULT 1,
	[Email] nvarchar(100),
	[BccEmail] nvarchar(100),
	[HtmlTemplate] nvarchar(MAX),
	[RedirectUrl] nvarchar(500),
	[SuccessMessage] nvarchar(500),
	[MetricsCode] nvarchar(50),
	[FileAttachmentEnabled] bit NOT NULL DEFAULT 0,
	[MaxFileAttachmentsCount] int NOT NULL DEFAULT 0, 
    [Title] NVARCHAR(200) NOT NULL, 
    [EmailTemplate] NVARCHAR(MAX) NULL

)
