CREATE TABLE [dbo].[Tags] (
    [Tag_id] INT          IDENTITY (1, 1) NOT NULL,
    [Name]   VARCHAR (20) NOT NULL,
    [Data]   VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Tag_id] ASC)
);

