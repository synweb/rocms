CREATE PROCEDURE [Shop].[Manufacturer_Update]
@Guid uniqueidentifier,
@Name nvarchar(250),
@LogoImageId varchar(30),
@Description nvarchar(MAX),
@Url nvarchar(250),
@CountryId int,
@ManufacturerId int
AS
	UPDATE [Shop].[Manufacturer] SET
		[Guid]=@Guid,
		[Name]=@Name,
		[LogoImageId]=@LogoImageId,
		[Description]=@Description,
		[Url]=@Url,
		[CountryId]=@CountryId
	WHERE [ManufacturerId]=@ManufacturerId
