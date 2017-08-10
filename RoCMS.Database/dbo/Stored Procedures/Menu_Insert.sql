CREATE PROCEDURE [dbo].[Menu_Insert]
	@Name nvarchar(200)
AS

INSERT INTO Menu ([Name]) Values (@Name)
SELECT @@IDENTITY
