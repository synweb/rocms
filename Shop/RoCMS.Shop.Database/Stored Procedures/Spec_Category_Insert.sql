CREATE PROCEDURE [Shop].[Spec_Category_Insert]
@CategoryId int,
@SpecId int
AS
	INSERT INTO [Shop].[Spec_Category] ([CategoryId], [SpecId])
	VALUES (@CategoryId, @SpecId)
