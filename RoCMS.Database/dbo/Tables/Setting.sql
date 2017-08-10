CREATE TABLE [dbo].[Setting] (
    [SettingId] INT            IDENTITY (1, 1) NOT NULL,
    [Key]       NVARCHAR (100)  NOT NULL,
    [Value]     NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Setting] PRIMARY KEY CLUSTERED ([SettingId] ASC),
    CONSTRAINT [AK_Setting] UNIQUE NONCLUSTERED ([Key] ASC)
);

