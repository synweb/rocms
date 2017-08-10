CREATE PROCEDURE [Shop].[Currency_Delete]
@CurrencyId varchar(3)
AS
	DELETE FROM [Shop].[Currency]
	WHERE [CurrencyId]=@CurrencyId
