CREATE TABLE [Shop].[Action] (
    [HeartId]     INT  NOT NULL,
    [DateOfEnding] DATETIME       NULL,
    [Description]  NVARCHAR (MAX) NULL,
    [Discount]     INT            NOT NULL,
    [Name] NVARCHAR(250) NULL, 
    [ImageId] VARCHAR(30) NULL, 
    [ShowInSlider] BIT NOT NULL DEFAULT 1, 
    [Active] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_ActionSet] PRIMARY KEY CLUSTERED ([HeartId] ASC),
	CONSTRAINT [FK_Actions_Image] FOREIGN KEY ([ImageId]) REFERENCES [dbo].[Image] ([ImageId]) ON UPDATE CASCADE ON DELETE SET NULL
);

