CREATE PROCEDURE [Shop].[PickUpPoint_Update]
@Title nvarchar(250),
@City nvarchar(100),
@Address nvarchar(500),
@Partner NVARCHAR(200), 
@Phone NVARCHAR(200), 
@Metro NVARCHAR(100), 
@Schedule NVARCHAR(200), 
@PaymentType NVARCHAR(200), 
@HowToReach NVARCHAR(1000), 
@Description NVARCHAR(1000), 
@ImageId VARCHAR(30),
@PickUpPointId int
AS
	UPDATE [Shop].[PickUpPoint] SET
		[Title]=@Title,
		[City]=@City,
		[Address]=@Address,
		[Partner]=@Partner,
		[Phone]=@Phone,
		[Metro]=@Metro,
		[Schedule]=@Schedule,
		[PaymentType]=@PaymentType,
		[HowToReach]=@HowToReach,
		[Description]=@Description,
		[ImageId]=@ImageId
	WHERE [PickUpPointId]=@PickUpPointId





