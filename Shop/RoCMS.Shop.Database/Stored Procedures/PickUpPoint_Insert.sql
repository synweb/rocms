CREATE PROCEDURE [Shop].[PickUpPoint_Insert]
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
@ImageId VARCHAR(30)
AS
	INSERT INTO [Shop].[PickUpPoint] ([Title], [City], [Address],[Partner],[Phone],[Metro],[Schedule],[PaymentType],[HowToReach],[Description],[ImageId])
	VALUES (@Title, @City, @Address, @Partner, @Phone, @Metro, @Schedule, @PaymentType, @HowToReach, @Description, @ImageId)
	SELECT @@IDENTITY
