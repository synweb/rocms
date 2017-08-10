CREATE PROCEDURE [dbo].[Setting_Upsert]
	@Key NVARCHAR (100),
	@Value NVARCHAR (MAX)
AS
	IF (EXISTS (SELECT * FROM [Setting] WHERE [Key]=@Key))
		UPDATE [Setting] SET [Value]=@Value
			WHERE [Key]=@Key
	ELSE
		INSERT INTO [Setting] ([Key],[Value]) VALUES (@Key, @Value)
