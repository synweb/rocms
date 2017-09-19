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
@EmailTemplate nvarchar(MAX),
@SendButtonText nvarchar(100),
@ClearButtonText nvarchar(100),
@HideClearButton BIT
AS
 INSERT INTO [dbo].[OrderForm] ([EmailSubject], [DateInEmailSubject], [Email], [BccEmail], [HtmlTemplate], [RedirectUrl], [SuccessMessage], [MetricsCode], [FileAttachmentEnabled], [MaxFileAttachmentsCount], [Title], [EmailTemplate], [SendButtonText], [ClearButtonText], [HideClearButton])
	VALUES (@EmailSubject, @DateInEmailSubject, @Email, @BccEmail, @HtmlTemplate, @RedirectUrl, @SuccessMessage, @MetricsCode, @FileAttachmentEnabled, @MaxFileAttachmentsCount, @Title, @EmailTemplate, @SendButtonText, @ClearButtonText, @HideClearButton)
	SELECT @@IDENTITY