CREATE PROCEDURE [Shop].[Currency_Insert]
@CurrencyId varchar(3),
@Name nvarchar(100),
@ShortName nvarchar(20),
@Rate decimal,
@SortOrder int,
@IsMain bit
AS
	INSERT INTO [Shop].[Currency] ([CurrencyId], [Name], [ShortName], [Rate], [SortOrder], [IsMain])
	VALUES (@CurrencyId, @Name, @ShortName, @Rate, @SortOrder, @IsMain)
