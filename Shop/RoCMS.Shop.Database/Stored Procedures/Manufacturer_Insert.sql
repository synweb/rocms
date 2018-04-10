CREATE PROCEDURE [Shop].[Manufacturer_Insert]
@HeartId int,
@Guid uniqueidentifier,
@Name nvarchar(250),
@LogoImageId varchar(30),
@Description nvarchar(MAX),
@CountryId int
AS
	INSERT INTO [Shop].[Manufacturer] ([HeartId], [Guid], [Name], [LogoImageId], [Description], [CountryId])
	VALUES (@HeartId, @Guid, @Name, @LogoImageId, @Description, @CountryId)
	SELECT @HeartId
