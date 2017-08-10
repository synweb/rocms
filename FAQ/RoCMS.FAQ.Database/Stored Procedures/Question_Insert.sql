CREATE PROCEDURE [FAQ].[Question_Insert]
@QuestionText nvarchar(MAX),
@AuthorId int,
@AuthorName nvarchar(200),
@AuthorEmail nvarchar(200),
@RespondentId int,
@AnswerText nvarchar(MAX),
@Moderated bit
AS
	INSERT INTO [FAQ].[Question] ([QuestionText], [AuthorId], [AuthorName], [AuthorEmail], [RespondentId], [AnswerText], [AnswerSentToAuthor], [Moderated])
	VALUES (@QuestionText, @AuthorId, @AuthorName, @AuthorEmail, @RespondentId, @AnswerText, 0, @Moderated)
	SELECT @@IDENTITY
