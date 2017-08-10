CREATE TABLE [dbo].[Image] (
    [ImageId]      VARCHAR(30)    NOT NULL,
    --[Image]        VARBINARY (MAX) NOT NULL,
    --[Thumbnail]    VARBINARY (MAX) NOT NULL,
    --[MimeType]     NVARCHAR (20)   NULL,
    [CreationDate] DATETIME        DEFAULT GETUTCDATE() NOT NULL,
    --[Size]         BIGINT          NULL,
	[InitialFilename] NVARCHAR(257) NULL,
    CONSTRAINT [PK_Image] PRIMARY KEY CLUSTERED ([ImageId] ASC)
);

