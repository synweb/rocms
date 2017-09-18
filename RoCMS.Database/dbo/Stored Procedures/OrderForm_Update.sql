﻿CREATE PROCEDURE [dbo].[OrderForm_Update]
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
@EmailTemplate nvarchar(MAX),
@SendButtonText nvarchar(100),
@ClearButtonText nvarchar(100),
@HideClearButton BIT
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
		[EmailTemplate]=@EmailTemplate,
		[SendButtonText]=@SendButtonText,
		[ClearButtonText]=@ClearButtonText,
		[HideClearButton]=@HideClearButton
	WHERE [OrderFormId]=@OrderFormId