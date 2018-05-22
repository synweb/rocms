CREATE PROCEDURE [Shop].[Pack_Insert]
@Name nvarchar(50),
@FullName nvarchar(200),
@Size float,
@DimensionId int,
@DefaultDiscount int
AS
	INSERT INTO [Shop].[Pack] ([Name], [Size], [DimensionId], [DefaultDiscount], [FullName])
	VALUES (@Name, @Size, @DimensionId, @DefaultDiscount, @FullName)
	SELECT @@IDENTITY
