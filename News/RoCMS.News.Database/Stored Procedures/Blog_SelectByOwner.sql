CREATE PROCEDURE [News].[Blog_SelectByOwner]
	@OwnerId int

AS

SELECT * FROM Blog WHERE OwnerId = @OwnerId