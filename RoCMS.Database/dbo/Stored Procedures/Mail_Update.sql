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
