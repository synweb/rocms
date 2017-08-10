CREATE TABLE [Shop].[GoodsReview]
(
	[GoodsReviewId] INT NOT NULL IDENTITY (1,1),
	[GoodsId] INT NOT NULL,
    [CreationDate]  DATETIME NOT NULL DEFAULT GETUTCDATE(),
	[Author] nvarchar(70) NOT NULL,
	[AuthorContact] nvarchar(150) NULL,
	[Rating] INT NULL,
	[Text] nvarchar(max) NULL,
	[UserId] INT NULL, 
    [Moderated] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_GoodsReview] PRIMARY KEY ([GoodsReviewId]),
	CONSTRAINT [Check_GoodsReview_Rating] CHECK ( [Rating] IS NULL OR ([Rating] >= 1 AND [Rating] <= 5)),
	CONSTRAINT [Check_GoodsReview_NotEmpty] CHECK ( [Rating] IS NOT NULL OR  [Text] IS NOT NULL),
	CONSTRAINT [FK_GoodsReview_Goods] FOREIGN KEY ([GoodsId]) REFERENCES [Shop].[GoodsItem] ([GoodsId]),
	CONSTRAINT [FK_GoodsReview_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
)
