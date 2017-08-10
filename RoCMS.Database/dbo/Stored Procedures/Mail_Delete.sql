CREATE PROCEDURE [dbo].[Mail_Delete]
	@MailId int
AS
	delete from [dbo].[Mail]
	where [MailId] = @MailId
