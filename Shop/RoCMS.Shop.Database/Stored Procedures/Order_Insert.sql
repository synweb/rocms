CREATE PROCEDURE [Shop].[Order_Insert]
@Address nvarchar(MAX),
@ClientId int,
@Comment nvarchar(MAX),
@AdminComment nvarchar(MAX),
@State nvarchar(20),
@ShipmentDate datetime,
@ShipmentType nvarchar(20),
@PickUpPointId int,
@DeliveryPrice decimal,
@TotalDiscount int,
@PaymentType varchar(20),
@PaymentState varchar(20)
AS
	INSERT INTO [Shop].[Order] ([Address], [ClientId], [Comment], [AdminComment], [State], [ShipmentDate], [ShipmentType], [PickUpPointId], [DeliveryPrice], [TotalDiscount], [PaymentType], [PaymentState])
	VALUES (@Address, @ClientId, @Comment, @AdminComment, @State, @ShipmentDate, @ShipmentType, @PickUpPointId, @DeliveryPrice, @TotalDiscount, @PaymentType, @PaymentState)
	SELECT @@IDENTITY
