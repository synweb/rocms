CREATE PROCEDURE [FAQ].[Question_SelectOne]
	@QuestionId int
AS
	SELECT [QuestionId], [CreationDate], [QuestionText], [AuthorId], [AuthorName], [AuthorEmail], [RespondentId], [AnswerText], [AnswerSentToAuthor], [Moderated], [SortOrder] FROM [FAQ].[Question]
	WHERE [QuestionId]=@QuestionId
