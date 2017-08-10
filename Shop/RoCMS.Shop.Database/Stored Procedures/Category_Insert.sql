CREATE PROCEDURE [Shop].[Category_Insert]
@Name nvarchar(MAX),
@ParentCategoryId int,
@Description nvarchar(MAX),
@SortOrder int,
@MetaDescription nvarchar(MAX),
@ImageId varchar(30),
@Hidden bit,
@RelativeUrl nvarchar(300)
AS
	INSERT INTO [Shop].[Category] ([Name], [ParentCategoryId], [Description], [SortOrder], [MetaDescription], [ImageId], [Hidden], [RelativeUrl])
	VALUES (@Name, @ParentCategoryId, @Description, @SortOrder, @MetaDescription, @ImageId, @Hidden, @RelativeUrl)
	SELECT @@IDENTITY
