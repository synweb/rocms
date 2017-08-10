CREATE PROCEDURE [dbo].[Mail_Select]
AS
	SELECT [MailId], [CreationDate], [Body], [Subject], [Receiver], [Sent], [ErrorMessage], [Attaches]
	from [dbo].[Mail]
	order by  [CreationDate] desc
