CREATE PROCEDURE [Shop].[Spec_Insert]
@Name nvarchar(250),
@Description nvarchar(MAX),
@ValueType nvarchar(50),
@AcceptableValues nvarchar(MAX),
@Prefix nvarchar(50),
@Postfix nvarchar(50),
@SortOrder int
AS
	INSERT INTO [Shop].[Spec] ([Name], [Description], [ValueType], [AcceptableValues], [Prefix], [Postfix], [SortOrder])
	VALUES (@Name, @Description, @ValueType, @AcceptableValues, @Prefix, @Postfix, @SortOrder)
	SELECT @@IDENTITY
