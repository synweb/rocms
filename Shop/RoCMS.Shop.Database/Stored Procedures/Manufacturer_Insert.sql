CREATE PROCEDURE [Shop].[Manufacturer_Insert]
@HeartId int,

@Name nvarchar(250),
@LogoImageId varchar(30),
@Description nvarchar(MAX),
@CountryId int
AS
	INSERT INTO [Shop].[Manufacturer] ([HeartId], [Name], [LogoImageId], [Description], [CountryId])
	VALUES (@HeartId, @Name, @LogoImageId, @Description, @CountryId)
	SELECT @HeartId
