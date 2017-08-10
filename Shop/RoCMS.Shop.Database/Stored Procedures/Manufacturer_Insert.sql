CREATE PROCEDURE [Shop].[Manufacturer_Insert]
@Guid uniqueidentifier,
@Name nvarchar(250),
@LogoImageId varchar(30),
@Description nvarchar(MAX),
@Url nvarchar(250),
@CountryId int
AS
	INSERT INTO [Shop].[Manufacturer] ([Guid], [Name], [LogoImageId], [Description], [Url], [CountryId])
	VALUES (@Guid, @Name, @LogoImageId, @Description, @Url, @CountryId)
	SELECT @@IDENTITY
