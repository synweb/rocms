CREATE PROCEDURE [dbo].[InterfaceString_Upsert]
	@Key varchar(200),
	@Value nvarchar(max)
AS
	IF EXISTS(SELECT * FROM InterfaceString WHERE [Key]=@Key)
		UPDATE InterfaceString SET [Value]=@Value WHERE [Key]=@Key
	ELSE
		INSERT INTO InterfaceString ([Key],[Value]) VALUES (@Key, @Value)
