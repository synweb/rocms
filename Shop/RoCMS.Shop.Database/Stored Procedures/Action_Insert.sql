CREATE PROCEDURE [Shop].[Action_Insert]
@DateOfEnding datetime,
@Description nvarchar(MAX),
@Discount int,
@Name nvarchar(250),
@ImageId varchar(30),
@ShowInSlider bit,
@Active bit
AS
	INSERT INTO [Shop].[Action] ([DateOfEnding], [Description], [Discount], [Name], [ImageId], [ShowInSlider], [Active])
	VALUES (@DateOfEnding, @Description, @Discount, @Name, @ImageId, @ShowInSlider, @Active)
	SELECT @@IDENTITY
