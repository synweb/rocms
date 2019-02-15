CREATE PROCEDURE [Shop].[Manufacturer_Update]

@Name nvarchar(250),
@LogoImageId varchar(30),
@Description nvarchar(MAX),
@CountryId int,
@HeartId int
AS
	UPDATE [Shop].[Manufacturer] SET

		[Name]=@Name,
		[LogoImageId]=@LogoImageId,
		[Description]=@Description,
		[CountryId]=@CountryId
	WHERE [HeartId]=@HeartId
