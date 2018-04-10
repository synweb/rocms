CREATE PROCEDURE [Shop].[Action_Insert]
@HeartId int,
@DateOfEnding datetime,
@Description nvarchar(MAX),
@Discount int,
@Name nvarchar(250),
@ImageId varchar(30),
@ShowInSlider bit,
@Active bit
AS
	INSERT INTO [Shop].[Action] ([HeartId], [DateOfEnding], [Description], [Discount], [Name], [ImageId], [ShowInSlider], [Active])
	VALUES (@HeartId, @DateOfEnding, @Description, @Discount, @Name, @ImageId, @ShowInSlider, @Active)
	SELECT @HeartId
