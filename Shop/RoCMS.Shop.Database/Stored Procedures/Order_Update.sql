CREATE PROCEDURE [Shop].[Order_Update]
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
@OrderId int,
@PaymentType varchar(20),
@PaymentState varchar(20)
AS
	UPDATE [Shop].[Order] SET
		[Address]=@Address,
		[ClientId]=@ClientId,
		[Comment]=@Comment,
		[AdminComment]=@AdminComment,
		[State]=@State,
		[ShipmentDate]=@ShipmentDate,
		[ShipmentType]=@ShipmentType,
		[PickUpPointId]=@PickUpPointId,
		[DeliveryPrice]=@DeliveryPrice,
		[TotalDiscount]=@TotalDiscount,
		[PaymentType]=@PaymentType,
		[PaymentState]=@PaymentState
	WHERE [OrderId]=@OrderId
