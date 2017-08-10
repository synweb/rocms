CREATE PROCEDURE [FAQ].[Question_Update]
@QuestionText nvarchar(MAX),
@RespondentId int,
@AnswerText nvarchar(MAX),
@AnswerSentToAuthor bit,
@Moderated bit,
@SortOrder int,
@QuestionId int
AS
	UPDATE [FAQ].[Question] SET
		[QuestionText]=@QuestionText,
		[RespondentId]=@RespondentId,
		[AnswerText]=@AnswerText,
		[AnswerSentToAuthor]=@AnswerSentToAuthor,
		[Moderated]=@Moderated,
		[SortOrder]=@SortOrder
	WHERE [QuestionId]=@QuestionId
