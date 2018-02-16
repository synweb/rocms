CREATE PROCEDURE [Shop].[GoodsAwaiting_Insert]
@HeartId int,
@Contact nvarchar(50),
@ContactType varchar(20),
@UserId int,
@Sent bit
AS
	INSERT INTO [Shop].[GoodsAwaiting] ([HeartId], [Contact], [ContactType], [UserId], [Sent])
	VALUES (@HeartId, @Contact, @ContactType, @UserId, @Sent)
	SELECT @@IDENTITY
