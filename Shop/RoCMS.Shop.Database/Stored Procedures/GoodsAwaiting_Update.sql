CREATE PROCEDURE [Shop].[GoodsAwaiting_Update]
@GoodsId int,
@Contact nvarchar(50),
@ContactType varchar(20),
@UserId int,
@Sent bit,
@GoodsAwaitingId int
AS
	UPDATE [Shop].[GoodsAwaiting] SET
		[GoodsId]=@GoodsId,
		[Contact]=@Contact,
		[ContactType]=@ContactType,
		[UserId]=@UserId,
		[Sent]=@Sent
	WHERE [GoodsAwaitingId]=@GoodsAwaitingId
