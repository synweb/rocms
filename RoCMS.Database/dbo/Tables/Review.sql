CREATE TABLE [dbo].[Review] (
    [ReviewId]  INT            IDENTITY (1, 1) NOT NULL,
    [Author]    NVARCHAR (50)  NOT NULL,
    [City]      NVARCHAR (50)  NULL,
    [Email]     NVARCHAR (100)  NULL,
    [Text]      NVARCHAR (MAX) NOT NULL,
	[Response]      NVARCHAR (MAX) NULL,
    [Moderated] BIT            NOT NULL DEFAULT 0,
    [VK] NVARCHAR(50) NULL, 
    [CreationDate] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT [PK_Review] PRIMARY KEY CLUSTERED ([ReviewId] DESC)
);

