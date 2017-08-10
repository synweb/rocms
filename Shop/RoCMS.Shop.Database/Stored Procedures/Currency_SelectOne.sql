CREATE PROCEDURE [Shop].[Currency_SelectOne]
@CurrencyId varchar(3)
AS
	SELECT * FROM [Shop].[Currency]
	WHERE [CurrencyId]=@CurrencyId
