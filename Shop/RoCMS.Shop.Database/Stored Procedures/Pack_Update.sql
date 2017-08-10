CREATE PROCEDURE [Shop].[Pack_Update]
@Guid uniqueidentifier,
@Name nvarchar(50),
@Size float,
@DimensionId int,
@DefaultDiscount int,
@PackId int
AS
	UPDATE [Shop].[Pack] SET
		[Guid]=@Guid,
		[Name]=@Name,
		[Size]=@Size,
		[DimensionId]=@DimensionId,
		[DefaultDiscount]=@DefaultDiscount
	WHERE [PackId]=@PackId
