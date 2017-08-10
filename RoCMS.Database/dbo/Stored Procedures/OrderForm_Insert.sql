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