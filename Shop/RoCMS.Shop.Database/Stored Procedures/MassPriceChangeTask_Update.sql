CREATE PROCEDURE [Shop].[MassPriceChangeTask_Update]
@Description nvarchar(MAX),
@State varchar(20),
@Comment nvarchar(MAX),
@TaskId int
AS
	UPDATE [Shop].[MassPriceChangeTask] SET
		[Description]=@Description,
		[State]=@State,
		[Comment]=@Comment
	WHERE [TaskId]=@TaskId
