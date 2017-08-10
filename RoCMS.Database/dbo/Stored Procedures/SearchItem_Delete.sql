CREATE PROCEDURE [dbo].[SearchItem_Delete]
	@EntityName varchar(200),
	@EntityId nvarchar(100)
AS
	DELETE FROM SearchItem WHERE [EntityName]=@EntityName AND [EntityId]=@EntityId
