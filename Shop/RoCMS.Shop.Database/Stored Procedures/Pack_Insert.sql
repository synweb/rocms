CREATE PROCEDURE [Shop].[Pack_Insert]
@Name nvarchar(50),
@Size float,
@DimensionId int,
@DefaultDiscount int
AS
	INSERT INTO [Shop].[Pack] ([Name], [Size], [DimensionId], [DefaultDiscount])
	VALUES (@Name, @Size, @DimensionId, @DefaultDiscount)
	SELECT @@IDENTITY
