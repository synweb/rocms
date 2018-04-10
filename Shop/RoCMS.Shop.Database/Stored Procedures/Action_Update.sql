CREATE PROCEDURE [Shop].[Action_Update]
@DateOfEnding datetime,
@Description nvarchar(MAX),
@Discount int,
@Name nvarchar(250),
@ImageId varchar(30),
@ShowInSlider bit,
@Active bit,
@HeartId int
AS
	UPDATE [Shop].[Action] SET
		[DateOfEnding]=@DateOfEnding,
		[Description]=@Description,
		[Discount]=@Discount,
		[Name]=@Name,
		[ImageId]=@ImageId,
		[ShowInSlider]=@ShowInSlider,
		[Active]=@Active
	WHERE [HeartId]=@HeartId
