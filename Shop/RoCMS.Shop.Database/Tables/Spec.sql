CREATE TABLE [Shop].[Spec] (
    [SpecId]      INT            IDENTITY (1, 1) NOT NULL,
    [CreationDate]  DATETIME NOT NULL DEFAULT GETUTCDATE(),
    [Guid]        UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() UNIQUE,
    [Name]        NVARCHAR (250) NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [ValueType]   NVARCHAR (50)  NOT NULL,
	[AcceptableValues] NVARCHAR(MAX) NULL,
	[Prefix] NVARCHAR(50) NULL, 
    [Postfix] NVARCHAR(50) NULL, 
	[SortOrder] INT NOT NULL DEFAULT 0,
    CONSTRAINT [PK_Spec] PRIMARY KEY CLUSTERED ([SpecId] ASC)
);

