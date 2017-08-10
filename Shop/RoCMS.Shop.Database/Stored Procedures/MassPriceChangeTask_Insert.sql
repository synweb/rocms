CREATE PROCEDURE [Shop].[MassPriceChangeTask_Insert]
@Description nvarchar(MAX),
@State varchar(20),
@Comment nvarchar(MAX)
AS
	INSERT INTO [Shop].[MassPriceChangeTask] ([Description], [State], [Comment])
	VALUES (@Description, @State, @Comment)
	SELECT @@IDENTITY
