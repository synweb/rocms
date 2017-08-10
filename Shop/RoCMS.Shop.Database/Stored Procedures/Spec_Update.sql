CREATE PROCEDURE [Shop].[Spec_Update]
@Name nvarchar(250),
@Description nvarchar(MAX),
@ValueType nvarchar(50),
@AcceptableValues nvarchar(MAX),
@Prefix nvarchar(50),
@Postfix nvarchar(50),
@SortOrder int,
@SpecId int
AS
	UPDATE [Shop].[Spec] SET
		[Name]=@Name,
		[Description]=@Description,
		[ValueType]=@ValueType,
		[AcceptableValues]=@AcceptableValues,
		[Prefix]=@Prefix,
		[Postfix]=@Postfix,
		[SortOrder]=@SortOrder
	WHERE [SpecId]=@SpecId
