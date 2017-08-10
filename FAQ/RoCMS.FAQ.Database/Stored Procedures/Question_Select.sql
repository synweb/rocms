CREATE PROCEDURE [FAQ].[Question_Select]
AS
	SELECT [QuestionId], [CreationDate], [QuestionText], [AuthorId], [AuthorName], [AuthorEmail], [RespondentId], [AnswerText], [AnswerSentToAuthor], [Moderated], [SortOrder] FROM [FAQ].[Question]
	ORDER BY SortOrder
