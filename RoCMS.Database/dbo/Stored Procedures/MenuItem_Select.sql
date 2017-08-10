CREATE PROCEDURE [dbo].[MenuItem_Select]
	@MenuId int

AS

SELEcT * FROM MenuItem
WHERE MenuId=@MenuId
