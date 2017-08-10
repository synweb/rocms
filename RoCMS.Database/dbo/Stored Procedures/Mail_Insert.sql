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