CREATE PROCEDURE [Shop].[Pack_Update]

@Name nvarchar(50),
@FullName nvarchar(200),
@Size float,
@DimensionId int,
@DefaultDiscount int,
@PackId int
AS
	UPDATE [Shop].[Pack] SET

		[Name]=@Name,
		[Size]=@Size,
		[DimensionId]=@DimensionId,
		[DefaultDiscount]=@DefaultDiscount,
		[FullName]=@FullName
	WHERE [PackId]=@PackId
