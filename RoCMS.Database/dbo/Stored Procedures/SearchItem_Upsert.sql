CREATE PROCEDURE [dbo].[SearchItem_Upsert]
	@SearchItemKey varchar(50),
	@EntityName varchar(200),
	@EntityId nvarchar(100),
	@SearchContent nvarchar(max),
	@Text nvarchar(500),
	@Title nvarchar(200),
	@Url nvarchar(500),
	@ImageId VARCHAR(30),
	@HeartId int,
	@Weight int,
	@StrictSearch BIT
AS
	IF (EXISTS(SELECT * FROM SearchItem WHERE [SearchItemKey]=@SearchItemKey AND [EntityName]=@EntityName AND [EntityId]=@EntityId))
		UPDATE [SearchItem]
			SET [SearchContent]=@SearchContent,
			[Text]=@Text,
			[Title]=@Title,
			[Url]=@Url,
			[Weight]=@Weight,
			[ImageId]=@ImageId,
			[StrictSearch]=@StrictSearch,
			[UpdateDate]=GETUTCDATE()
		WHERE 
			[SearchItemKey]=@SearchItemKey AND [EntityName]=@EntityName AND [EntityId]=@EntityId
	ELSE
		INSERT INTO [SearchItem] ([SearchItemKey], [EntityName], [EntityId], [SearchContent], [Text], [Title], [Url], [Weight],[ImageId],[HeartId], [StrictSearch])
			VALUES (@SearchItemKey, @EntityName, @EntityId, @SearchContent, @Text, @Title, @Url, @Weight, @ImageId, @HeartId, @StrictSearch)
		