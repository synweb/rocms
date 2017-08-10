CREATE PROCEDURE [dbo].[Mail_SelectOne]
	@MailId int
AS
	SELECT [MailId], [CreationDate], [Body], [Subject], [Receiver], [Sent], [ErrorMessage], [Attaches]
	from [dbo].[Mail]
	where [MailId] = @MailId