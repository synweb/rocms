CREATE PROCEDURE [Shop].[GoodsAwaiting_Update]
@HeartId int,
@Contact nvarchar(50),
@ContactType varchar(20),
@UserId int,
@Sent bit,
@GoodsAwaitingId int
AS
	UPDATE [Shop].[GoodsAwaiting] SET
		[HeartId]=@HeartId,
		[Contact]=@Contact,
		[ContactType]=@ContactType,
		[UserId]=@UserId,
		[Sent]=@Sent
	WHERE [GoodsAwaitingId]=@GoodsAwaitingId
