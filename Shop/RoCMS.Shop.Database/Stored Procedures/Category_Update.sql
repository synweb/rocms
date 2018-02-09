CREATE PROCEDURE [Shop].[Category_Update]
@Guid uniqueidentifier,
@Name nvarchar(MAX),
@ParentCategoryId int,
@Description nvarchar(MAX),
@SortOrder int,
@MetaDescription nvarchar(MAX),
@ImageId varchar(30),
@Hidden bit,
@RelativeUrl nvarchar(300),
@HeartId int
AS
	UPDATE [Shop].[Category] SET
		[Guid]=@Guid,
		[Name]=@Name,
		[ParentCategoryId]=@ParentCategoryId,
		[Description]=@Description,
		[SortOrder]=@SortOrder,
		
		[ImageId]=@ImageId,
		[Hidden]=@Hidden
		
	WHERE [HeartId]=@HeartId
