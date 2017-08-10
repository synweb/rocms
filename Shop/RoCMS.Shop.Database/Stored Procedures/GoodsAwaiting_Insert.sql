CREATE PROCEDURE [Shop].[GoodsAwaiting_Insert]
@GoodsId int,
@Contact nvarchar(50),
@ContactType varchar(20),
@UserId int,
@Sent bit
AS
	INSERT INTO [Shop].[GoodsAwaiting] ([GoodsId], [Contact], [ContactType], [UserId], [Sent])
	VALUES (@GoodsId, @Contact, @ContactType, @UserId, @Sent)
	SELECT @@IDENTITY
