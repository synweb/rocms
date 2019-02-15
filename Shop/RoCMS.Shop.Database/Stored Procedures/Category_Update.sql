CREATE PROCEDURE [Shop].[Category_Update]

@Name nvarchar(MAX),
@ParentCategoryId int,
@Description nvarchar(MAX),
@SortOrder int,

@ImageId varchar(30),
@Hidden bit,

@HeartId int
AS
	UPDATE [Shop].[Category] SET

		[Name]=@Name,
		[ParentCategoryId]=@ParentCategoryId,
		[Description]=@Description,
		[SortOrder]=@SortOrder,
		
		[ImageId]=@ImageId,
		[Hidden]=@Hidden
		
	WHERE [HeartId]=@HeartId
