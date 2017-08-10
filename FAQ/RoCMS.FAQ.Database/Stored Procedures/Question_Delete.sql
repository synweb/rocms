CREATE PROCEDURE [FAQ].[Question_Delete]
@QuestionId int
AS
	DELETE FROM [FAQ].[Question]
	WHERE [QuestionId]=@QuestionId
