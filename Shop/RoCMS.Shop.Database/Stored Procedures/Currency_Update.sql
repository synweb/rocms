CREATE PROCEDURE [Shop].[Currency_Update]
@CurrencyId varchar(3),
@Name nvarchar(100),
@ShortName nvarchar(20),
@Rate decimal,
@SortOrder int,
@IsMain bit
AS
	UPDATE [Shop].[Currency] SET
		[Name]=@Name,
		[ShortName]=@ShortName,
		[Rate]=@Rate,
		[SortOrder]=@SortOrder,
		[IsMain]=@IsMain
		WHERE [CurrencyId]=@CurrencyId